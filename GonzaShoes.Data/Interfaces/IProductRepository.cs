using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsAsync();
        Task SaveProductAsync(Product product);
    }
}
