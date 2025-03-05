using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandByIdAsync(int id);
        Task<Brand?> GetBrandByNameAsync(string name);
        Task<List<Brand>> GetBrandsAsync(BrandSearchDTO searchDTO);
        Task<List<NameIdDTO>> GetNameIdDTOsAsync();
        Task SaveBrandAsync(Brand brand);
        Task UpdateStatusAsync(Brand obj);
    }
}
