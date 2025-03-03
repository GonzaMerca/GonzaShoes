using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IColorRepository
    {
        Task<Color?> GetColorByIdAsync(int id);
        Task<List<Color>> GetColorsAsync();
        Task SaveColorAsync(Color color);
    }
}
