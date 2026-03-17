using Clothify.Application.DTOs.Response;
using Clothify.Application.Services;
using Clothify.Core.Enums;
using Clothify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clothify.Web.Pages.Admin;

[Authorize(Roles = "Admin")]
public class AdminDashboardModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderService _orderService;

    public AdminDashboardModel(IUnitOfWork unitOfWork, IOrderService orderService)
    {
        _unitOfWork = unitOfWork;
        _orderService = orderService;
    }

    public AdminDashboardResponse? Dashboard { get; set; }

    public async Task OnGetAsync()
    {
        await LoadDashboard();
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, string newStatus)
    {
        if (Enum.TryParse<OrderStatus>(newStatus, out var status))
        {
            await _orderService.UpdateOrderStatusAsync(orderId, status);
        }
        await LoadDashboard();
        return Page();
    }

    private async Task LoadDashboard()
    {
        try
        {
            var from = DateTime.UtcNow.AddDays(-30);
            var to = DateTime.UtcNow;

            var revenue = await _unitOfWork.Orders.GetRevenueAsync(from, to);
            var orderCount = await _unitOfWork.Orders.GetOrderCountAsync(from, to);
            var recentOrders = await _unitOfWork.Orders.GetRecentOrdersAsync(20);
            var lowStock = await _unitOfWork.Products.GetLowStockProductsAsync(5);

            Dashboard = new AdminDashboardResponse
            {
                Revenue = new KpiCard { Label = "Revenue", Value = $"Rs. {revenue:N0}" },
                TotalOrders = new KpiCard { Label = "Orders", Value = orderCount.ToString() },
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
        }
        catch { }
    }
}
