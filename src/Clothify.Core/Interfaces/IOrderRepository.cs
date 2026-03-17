using Clothify.Core.Entities;
using Clothify.Core.Enums;

namespace Clothify.Core.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetUserOrdersAsync(string userId, int page, int pageSize);
    Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, int page, int pageSize);
    Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
    Task<string> GenerateOrderNumberAsync();
    Task<int> GetOrderCountAsync(DateTime from, DateTime to);
    Task<decimal> GetRevenueAsync(DateTime from, DateTime to);
}
