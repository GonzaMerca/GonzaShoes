using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Product;

namespace GonzaShoes.Service.Interfaces
{
    public interface IProductService : IBaseService
    {
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO?> GetProductByNameAsync(string name);
        Task<List<ProductDTO>> GetProductsAsync(ProductSearchDTO searchDTO);
        Task<ValidationResultDTO> SaveProductAsync(ProductDTO Product);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
        Task UpdateStockAsync(int productId, int quantity, int itemId, int orderId, string description, bool isDecreasingStock = true);
        Task<bool> ValidateStockAsync(int id, int quantity);
        Task<bool> IsAnyProductByBrandAsync(int brandId);
        Task<bool> IsAnyProductByModelProductAsync(int modelProduct);
        Task<bool> IsAnyProductByColorAsync(int colorId);
        Task<bool> IsAnyProductBySizeAsync(int sizeId);
    }
}