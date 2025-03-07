using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product?> GetProductByNameAsync(string name);
        Task<List<Product>> GetProductsAsync(ProductSearchDTO searchDTO);
        Task SaveProductAsync(Product product);
        Task UpdateStatusAsync(Product obj);
        Task<bool> ValidateStockAsync(int productId, int quantity);
        Task<bool> IsAnyProductAsync(ProductDTO product);
        Task<bool> IsAnyProductByBrandAsync(int brandId);
        Task<bool> IsAnyProductByModelProductAsync(int modelProduct);
        Task<bool> IsAnyProductByColorAsync(int colorId);
        Task<bool> IsAnyProductBySizeAsync(int sizeId);
    }
}
