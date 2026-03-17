namespace Clothify.Application.DTOs.Response;

public class AdminDashboardResponse
{
    public KpiCard Revenue { get; set; } = new();
    public KpiCard TotalOrders { get; set; } = new();
    public KpiCard NewCustomers { get; set; } = new();
    public KpiCard ConversionRate { get; set; } = new();
    public List<RevenueDataPoint> RevenueChart { get; set; } = new();
    public List<AdminOrderSummary> RecentOrders { get; set; } = new();
    public List<TopProductResponse> TopProducts { get; set; } = new();
    public List<LowStockAlert> LowStockAlerts { get; set; } = new();
}

public class KpiCard
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public decimal ChangePercent { get; set; }
    public bool IsPositive { get; set; }
}

public class RevenueDataPoint
{
    public string Date { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class AdminOrderSummary
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class TopProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int UnitsSold { get; set; }
    public decimal Revenue { get; set; }
}

public class LowStockAlert
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string VariantInfo { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
}
