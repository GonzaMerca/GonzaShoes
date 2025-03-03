using GonzaShoes.Model.DTOs;

namespace GonzaShoes.Service.Interfaces
{
    public interface IAccountService
    {
        Task<LoginResultDTO> LoginUserAsync(string email, string password);
    }
}
