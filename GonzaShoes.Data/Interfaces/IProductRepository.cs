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
    }
}
