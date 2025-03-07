using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.User;

namespace GonzaShoes.Service.Interfaces
{
    public interface IUserService : IBaseService
    {
        Task<UserDTO?> GetUserByEmailAsync(string emailAddress);
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<List<UserDTO>> GetUsersAsync(UserSearchDTO searchDTO);
        Task<List<NameIdDTO>> GetNameIdDTOsAsync();
        Task<ValidationResultDTO> SaveUserAsync(UserDTO user);
        Task<ValidationResultDTO> UpdateStatusAsync(int id, bool isActive);
    }
}