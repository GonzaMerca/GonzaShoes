using GonzaShoes.Model.Entities.Product;

namespace GonzaShoes.Data.Interfaces
{
    public interface ISizeRepository
    {
        Task<Size?> GetSizeByIdAsync(int id);
        Task<List<Size>> GetSizesAsync();
        Task SaveSizeAsync(Size size);
    }
}
