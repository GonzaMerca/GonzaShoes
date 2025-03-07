using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IProductStockFlowRepository
    {
        Task<ProductStockFlow?> GetProductStockFlowByIdAsync(int id);
        Task<List<ProductStockFlow>> GetProductStockFlowsAsync();
        Task SaveProductStockFlowsAsync(ProductStockFlow obj);
        Task UpdateStatusAsync(ProductStockFlow obj);
    }
}
