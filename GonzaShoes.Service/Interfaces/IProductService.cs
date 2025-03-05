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
        Task<bool> IsAnyProductByBrandAsync(int brandId);
        Task<bool> IsAnyProductByModelProductAsync(int modelProduct);
        Task<bool> IsAnyProductByColorAsync(int colorId);
        Task<bool> IsAnyProductBySizeAsync(int sizeId);
    }
}