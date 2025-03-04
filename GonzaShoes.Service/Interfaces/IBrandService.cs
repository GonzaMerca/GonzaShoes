using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Brand;

namespace GonzaShoes.Service.Interfaces
{
    public interface IBrandService : IBaseService
    {
        Task<BrandDTO?> GetBrandByIdAsync(int id);
        Task<BrandDTO?> GetBrandByNameAsync(string name);
        Task<List<BrandDTO>> GetBrandsAsync(BrandSearchDTO searchDTO);
        Task<ValidationResultDTO> SaveBrandAsync(BrandDTO Brand);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}