using GonzaShoes.Model.Entities.Order;

namespace GonzaShoes.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersAsync();
        Task SaveOrderAsync(Order order);
    }
}
