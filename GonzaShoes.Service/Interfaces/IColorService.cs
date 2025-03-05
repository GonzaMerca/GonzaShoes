using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Color;

namespace GonzaShoes.Service.Interfaces
{
    public interface IColorService : IBaseService
    {
        Task<ColorDTO?> GetColorByIdAsync(int id);
        Task<ColorDTO?> GetColorByNameAsync(string name);
        Task<List<ColorDTO>> GetColorsAsync(ColorSearchDTO searchDTO);
        Task<List<NameIdDTO>> GetNameIdDTOsAsync();
        Task<ValidationResultDTO> SaveColorAsync(ColorDTO Color);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}