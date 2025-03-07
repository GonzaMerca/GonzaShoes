using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.User;
using GonzaShoes.Model.Entities.User;

namespace GonzaShoes.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string emailAddress);
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetUsersAsync(UserSearchDTO searchDTO);
        Task<List<NameIdDTO>> GetNameIdDTOsAsync();
        Task SaveUserAsync(User user);
        Task UpdateStatusAsync(User obj);
    }
}
