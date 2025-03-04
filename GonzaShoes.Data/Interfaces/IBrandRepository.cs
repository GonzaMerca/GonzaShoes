using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<Brand?> GetBrandByNameAsync(string name);
        Task<List<Brand>> GetBrandsAsync(BrandSearchDTO searchDTO);
        Task SaveBrandAsync(Brand brand);
        Task UpdateStatusAsync(Brand obj);
    }
}
