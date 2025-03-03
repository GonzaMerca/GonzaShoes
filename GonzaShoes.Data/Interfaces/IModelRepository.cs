using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IModelRepository
    {
        Task<ModelProduct?> GetModelByIdAsync(int id);
        Task<List<ModelProduct>> GetModelsAsync();
        Task SaveModelAsync(ModelProduct model);
    }
}
