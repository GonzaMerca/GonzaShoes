using GonzaShoes.Model.DTOs.Order;
using GonzaShoes.Model.Entities.Order;

namespace GonzaShoes.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersAsync(OrderSearchDTO searchDTO);
        Task SaveOrderAsync(Order order);
        Task UpdateStatusAsync(Order obj);
    }
}
