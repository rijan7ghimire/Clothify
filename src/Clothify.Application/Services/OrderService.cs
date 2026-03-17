using AutoMapper;
using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Core.Entities;
using Clothify.Core.Enums;
using Clothify.Core.Exceptions;
using Clothify.Core.Interfaces;

namespace Clothify.Application.Services;

public interface IOrderService
{
    Task<OrderResponse> PlaceOrderAsync(string userId, PlaceOrderRequest request);
    Task<OrderResponse> GetOrderAsync(int id);
    Task<PagedResponse<OrderResponse>> GetUserOrdersAsync(string userId, int page, int pageSize);
    Task<OrderTrackingResponse> GetOrderTrackingAsync(int id);
    Task CancelOrderAsync(int id, string userId);
    Task UpdateOrderStatusAsync(int id, OrderStatus status);
}

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OrderResponse> PlaceOrderAsync(string userId, PlaceOrderRequest request)
    {
        var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
        if (cart == null || !cart.Items.Any())
            throw new BadRequestException("Cart is empty");

        int addressId;
        if (request.ShippingAddressId.HasValue)
        {
            addressId = request.ShippingAddressId.Value;
        }
        else if (request.NewAddress != null)
        {
            var address = new Address
            {
                UserId = userId,
                FullName = request.NewAddress.FullName,
                StreetLine1 = request.NewAddress.StreetLine1,
                StreetLine2 = request.NewAddress.StreetLine2,
                City = request.NewAddress.City,
                State = request.NewAddress.State,
                ZipCode = request.NewAddress.ZipCode,
                Country = request.NewAddress.Country,
                Phone = request.NewAddress.Phone,
                IsDefault = request.NewAddress.IsDefault
            };
            await _unitOfWork.Repository<Address>().AddAsync(address);
            await _unitOfWork.SaveChangesAsync();
            addressId = address.Id;
        }
        else
        {
            throw new BadRequestException("Shipping address is required");
        }

        var orderNumber = await _unitOfWork.Orders.GenerateOrderNumberAsync();
        var shippingCost = request.ShippingMethod switch
        {
            ShippingMethod.Express => 500m,
            ShippingMethod.NextDay => 1500m,
            _ => 0m
        };

        var subtotal = cart.Items.Sum(i =>
            (i.ProductVariant.PriceOverride ?? i.ProductVariant.Product.DiscountPrice ?? i.ProductVariant.Product.BasePrice) * i.Quantity);
        var tax = Math.Round(subtotal * 0.13m, 2);

        var order = new Order
        {
            UserId = userId,
            OrderNumber = orderNumber,
            ShippingAddressId = addressId,
            ShippingMethod = request.ShippingMethod,
            PaymentMethod = request.PaymentMethod,
            Subtotal = subtotal,
            ShippingCost = shippingCost,
            Tax = tax,
            Total = subtotal + shippingCost + tax,
            CouponCode = request.CouponCode,
            EstimatedDeliveryDate = request.ShippingMethod switch
            {
                ShippingMethod.NextDay => DateTime.UtcNow.AddDays(1),
                ShippingMethod.Express => DateTime.UtcNow.AddDays(3),
                _ => DateTime.UtcNow.AddDays(7)
            }
        };

        foreach (var item in cart.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductVariantId = item.ProductVariantId,
                ProductName = item.ProductVariant.Product.Name,
                Size = item.ProductVariant.Size,
                Color = item.ProductVariant.Color,
                ImageUrl = item.ProductVariant.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl,
                Quantity = item.Quantity,
                UnitPrice = item.ProductVariant.PriceOverride ?? item.ProductVariant.Product.DiscountPrice ?? item.ProductVariant.Product.BasePrice,
                Total = (item.ProductVariant.PriceOverride ?? item.ProductVariant.Product.DiscountPrice ?? item.ProductVariant.Product.BasePrice) * item.Quantity
            });

            // Decrease stock
            item.ProductVariant.StockQuantity -= item.Quantity;
        }

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.Cart.ClearCartAsync(cart.Id);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<OrderResponse> GetOrderAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(id);
        if (order == null) throw new NotFoundException(nameof(Order), id);
        return _mapper.Map<OrderResponse>(order);
    }

    public async Task<PagedResponse<OrderResponse>> GetUserOrdersAsync(string userId, int page, int pageSize)
    {
        var orders = await _unitOfWork.Orders.GetUserOrdersAsync(userId, page, pageSize);
        var totalCount = await _unitOfWork.Orders.CountAsync(o => o.UserId == userId);
        return new PagedResponse<OrderResponse>
        {
            Items = _mapper.Map<List<OrderResponse>>(orders),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<OrderTrackingResponse> GetOrderTrackingAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(id);
        if (order == null) throw new NotFoundException(nameof(Order), id);

        var steps = new List<TrackingStep>
        {
            new() { Title = "Order Placed", CompletedAt = order.PlacedAt, IsCompleted = true },
            new() { Title = "Payment Confirmed", CompletedAt = order.PlacedAt, IsCompleted = order.Status >= OrderStatus.Confirmed },
            new() { Title = "Processing", IsCompleted = order.Status >= OrderStatus.Processing },
            new() { Title = "Shipped", CompletedAt = order.ShippedAt, IsCompleted = order.Status >= OrderStatus.Shipped },
            new() { Title = "Out for Delivery", IsCompleted = order.Status >= OrderStatus.OutForDelivery },
            new() { Title = "Delivered", CompletedAt = order.DeliveredAt, IsCompleted = order.Status >= OrderStatus.Delivered }
        };

        var currentStep = steps.LastOrDefault(s => s.IsCompleted);
        if (currentStep != null) currentStep.IsCurrent = true;

        return new OrderTrackingResponse
        {
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            TrackingNumber = order.TrackingNumber,
            CarrierName = order.CarrierName,
            EstimatedDeliveryDate = order.EstimatedDeliveryDate,
            ShippingAddress = $"{order.ShippingAddress.StreetLine1}, {order.ShippingAddress.City}, {order.ShippingAddress.State}",
            Steps = steps
        };
    }

    public async Task CancelOrderAsync(int id, string userId)
    {
        var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(id);
        if (order == null) throw new NotFoundException(nameof(Order), id);
        if (order.UserId != userId) throw new BadRequestException("Unauthorized");
        if (order.Status > OrderStatus.Processing)
            throw new BadRequestException("Order cannot be cancelled at this stage");

        order.Status = OrderStatus.Cancelled;
        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateOrderStatusAsync(int id, OrderStatus status)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) throw new NotFoundException(nameof(Order), id);

        order.Status = status;
        if (status == OrderStatus.Shipped) order.ShippedAt = DateTime.UtcNow;
        if (status == OrderStatus.Delivered) order.DeliveredAt = DateTime.UtcNow;

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();
    }
}
