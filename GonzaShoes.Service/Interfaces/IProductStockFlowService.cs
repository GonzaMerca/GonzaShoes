using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Service.Interfaces
{
    public interface IProductStockFlowService : IBaseService
    {
        Task<ProductStockFlowDTO?> GetProductStockFlowByIdAsync(int id);
        Task<List<ProductStockFlowDTO>> GetProductStockFlowsAsync();
        Task<ValidationResultDTO> SaveProductStockFlowsAsync(ProductStockFlowDTO obj);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}