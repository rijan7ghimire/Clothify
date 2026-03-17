using Clothify.Application.DTOs.Request;
using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Clothify.Core.Entities;
using Clothify.Core.Enums;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clothify.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(
        IUnitOfWork unitOfWork,
        IProductService productService,
        IOrderService orderService,
        UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
        _orderService = orderService;
        _userManager = userManager;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard([FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var startDate = from ?? DateTime.UtcNow.AddDays(-30);
        var endDate = to ?? DateTime.UtcNow;
        var prevStart = startDate.AddDays(-(endDate - startDate).Days);

        var revenue = await _unitOfWork.Orders.GetRevenueAsync(startDate, endDate);
        var prevRevenue = await _unitOfWork.Orders.GetRevenueAsync(prevStart, startDate);
        var orderCount = await _unitOfWork.Orders.GetOrderCountAsync(startDate, endDate);
        var prevOrderCount = await _unitOfWork.Orders.GetOrderCountAsync(prevStart, startDate);

        var recentOrders = await _unitOfWork.Orders.GetRecentOrdersAsync(10);
        var lowStock = await _unitOfWork.Products.GetLowStockProductsAsync(5);

        var dashboard = new AdminDashboardResponse
        {
            Revenue = new KpiCard
            {
                Label = "Revenue",
                Value = $"${revenue:N2}",
                ChangePercent = prevRevenue > 0 ? ((revenue - prevRevenue) / prevRevenue) * 100 : 0,
                IsPositive = revenue >= prevRevenue
            },
            TotalOrders = new KpiCard
            {
                Label = "Orders",
                Value = orderCount.ToString(),
                ChangePercent = prevOrderCount > 0 ? ((decimal)(orderCount - prevOrderCount) / prevOrderCount) * 100 : 0,
                IsPositive = orderCount >= prevOrderCount
            },
            RecentOrders = recentOrders.Select(o => new AdminOrderSummary
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerName = $"{o.User.FirstName} {o.User.LastName}",
                ItemCount = o.Items.Sum(i => i.Quantity),
                Total = o.Total,
                Status = o.Status.ToString(),
                Date = o.PlacedAt
            }).ToList(),
            LowStockAlerts = lowStock.SelectMany(p => p.Variants
                .Where(v => v.StockQuantity <= 5)
                .Select(v => new LowStockAlert
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    VariantInfo = $"{v.Size} / {v.Color}",
                    CurrentStock = v.StockQuantity
                })).ToList()
        };

        return Ok(dashboard);
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct([FromBody] AdminCreateProductRequest request)
    {
        var result = await _productService.CreateProductAsync(request);
        return Created($"/api/products/{result.Id}", result);
    }

    [HttpPut("products/{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] AdminCreateProductRequest request)
    {
        await _productService.UpdateProductAsync(id, request);
        return NoContent();
    }

    [HttpDelete("products/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }

    [HttpPut("orders/{id:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        await _orderService.UpdateOrderStatusAsync(id, request.Status);
        return NoContent();
    }

    [HttpPost("coupons")]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest request)
    {
        var coupon = new Coupon
        {
            Code = request.Code.ToUpper(),
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            MinOrderAmount = request.MinOrderAmount,
            MaxUses = request.MaxUses,
            ExpiresAt = request.ExpiresAt
        };
        await _unitOfWork.Repository<Coupon>().AddAsync(coupon);
        await _unitOfWork.SaveChangesAsync();
        return Created("", coupon);
    }
}

public class UpdateStatusRequest
{
    public OrderStatus Status { get; set; }
}
