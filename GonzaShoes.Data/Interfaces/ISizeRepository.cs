using GonzaShoes.Model.DTOs.Size;
using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface ISizeRepository
    {
        Task<Size?> GetSizeByIdAsync(int id);
        Task<Size?> GetSizeByNumberAsync(decimal number);
        Task<List<Size>> GetSizesAsync(SizeSearchDTO searchDTO);
        Task SaveSizeAsync(Size size);
        Task UpdateStatusAsync(Size obj);
    }
}
