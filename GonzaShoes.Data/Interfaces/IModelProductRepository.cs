using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IModelProductRepository
    {
        Task<ModelProduct?> GetModelProductByIdAsync(int id);
        Task<ModelProduct?> GetModelProductByNameAsync(string name);
        Task<List<ModelProduct>> GetModelProductsAsync(ModelProductSearchDTO searchDTO);
        Task SaveModelProductAsync(ModelProduct model);
        Task UpdateStatusAsync(ModelProduct obj);
    }
}
