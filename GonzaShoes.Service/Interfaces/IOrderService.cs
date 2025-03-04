using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Order;

namespace GonzaShoes.Service.Interfaces
{
    public interface IOrderService : IBaseService
    {
        Task<OrderDTO?> GetOrderByIdAsync(int id);
        Task<List<OrderDTO>> GetOrdersAsync(OrderSearchDTO searchDTO);
        Task<ValidationResultDTO> SaveOrderAsync(OrderDTO Order);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}