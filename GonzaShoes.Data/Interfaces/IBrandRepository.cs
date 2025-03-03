using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<List<Brand>> GetBrandsAsync();
        Task SaveBrandAsync(Brand brand);
    }
}
