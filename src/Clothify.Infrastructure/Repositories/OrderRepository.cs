using Clothify.Core.Entities;
using Clothify.Core.Enums;
using Clothify.Core.Interfaces;
using Clothify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clothify.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context) { }

    public async Task<Order?> GetOrderWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Items)
                .ThenInclude(oi => oi.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                        .ThenInclude(p => p.Images.Where(i => i.IsMain))
            .Include(o => o.ShippingAddress)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId, int page, int pageSize)
    {
        return await _dbSet
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.PlacedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status, int page, int pageSize)
    {
        return await _dbSet
            .Where(o => o.Status == status)
            .Include(o => o.User)
            .OrderByDescending(o => o.PlacedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count)
    {
        return await _dbSet
            .Include(o => o.User)
            .Include(o => o.Items)
            .OrderByDescending(o => o.PlacedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<string> GenerateOrderNumberAsync()
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        var count = await _dbSet.CountAsync(o => o.PlacedAt.Date == DateTime.UtcNow.Date) + 1;
        return $"CLT-{date}-{count:D4}";
    }

    public async Task<int> GetOrderCountAsync(DateTime from, DateTime to)
        => await _dbSet.CountAsync(o => o.PlacedAt >= from && o.PlacedAt <= to);

    public async Task<decimal> GetRevenueAsync(DateTime from, DateTime to)
        => await _dbSet.Where(o => o.PlacedAt >= from && o.PlacedAt <= to
            && o.Status != OrderStatus.Cancelled).SumAsync(o => o.Total);
}
