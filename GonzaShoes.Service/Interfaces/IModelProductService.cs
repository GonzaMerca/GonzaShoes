using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.ModelProduct;

namespace GonzaShoes.Service.Interfaces
{
    public interface IModelProductService : IBaseService
    {
        Task<ModelProductDTO?> GetModelProductByIdAsync(int id);
        Task<ModelProductDTO?> GetModelProductByNameAsync(string name);
        Task<List<ModelProductDTO>> GetModelProductsAsync(ModelProductSearchDTO searchDTO);
        Task<ValidationResultDTO> SaveModelProductAsync(ModelProductDTO ModelProduct);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}