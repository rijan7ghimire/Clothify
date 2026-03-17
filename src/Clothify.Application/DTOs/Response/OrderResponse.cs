using Clothify.Core.Enums;

namespace Clothify.Application.DTOs.Response;

public class OrderResponse
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public string StatusDisplay { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
    public DateTime PlacedAt { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public List<OrderItemResponse> Items { get; set; } = new();
}

public class OrderItemResponse
{
    public string ProductName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
}

public class OrderTrackingResponse
{
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public string? TrackingNumber { get; set; }
    public string? CarrierName { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public List<TrackingStep> Steps { get; set; } = new();
}

public class TrackingStep
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCurrent { get; set; }
}
