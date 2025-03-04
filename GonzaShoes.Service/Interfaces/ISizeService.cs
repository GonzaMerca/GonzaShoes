using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Size;

namespace GonzaShoes.Service.Interfaces
{
    public interface ISizeService : IBaseService
    {
        Task<SizeDTO?> GetSizeByIdAsync(int id);
        Task<SizeDTO?> GetSizeByNumberAsync(decimal number);
        Task<List<SizeDTO>> GetSizesAsync(SizeSearchDTO searchDTO);
        Task<ValidationResultDTO> SaveSizeAsync(SizeDTO Size);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}