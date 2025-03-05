using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Color;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface IColorRepository
    {
        Task<Color?> GetColorByIdAsync(int id);
        Task<Color?> GetColorByNameAsync(string name);
        Task<List<Color>> GetColorsAsync(ColorSearchDTO searchDTO);
        Task<List<NameIdDTO>> GetNameIdDTOsAsync();
        Task SaveColorAsync(Color color);
        Task UpdateStatusAsync(Color obj);
    }
}
