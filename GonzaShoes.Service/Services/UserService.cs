using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.User;
using GonzaShoes.Model.Entities.User;
using GonzaShoes.Model.Helper;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;

            this.mapper = mapper;
        }

        public async Task<UserDTO?> GetUserByEmailAsync(string emailAddress)
        {
            return this.mapper.Map<User, UserDTO>(await this.userRepository.GetUserByEmailAsync(emailAddress));
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            return this.mapper.Map<User, UserDTO>(await this.userRepository.GetUserByIdAsync(id));
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            return this.mapper.Map<List<User>, List<UserDTO>>(await this.userRepository.GetUsersAsync());
        }

        public async Task<ValidationResultDTO> SaveUserAsync(UserDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveUserAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            User? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<UserDTO, User>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.userRepository.GetUserByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            obj.Password = SecurePasswordHasher.HashPassword(obj.Password);

            await this.userRepository.SaveUserAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveUserAsync(UserDTO user)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(user.Name))
                validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre del usuario");
            else if (user.Name.Length > 50)
                validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");

            if (string.IsNullOrWhiteSpace(user.Password))
                validationResultDTO.ErrorMessages.Add("Falta ingresar la contraseña del usuario");
            else if (user.Password.Length > 50)
                validationResultDTO.ErrorMessages.Add("La contraseña no puede tener mas de 50 caracteres");

            if (string.IsNullOrWhiteSpace(user.Email))
                validationResultDTO.ErrorMessages.Add("Falta ingresar la dirección de correo electrónico del usuario");
            else
            {
                if (user.Email.Length > 100)
                    validationResultDTO.ErrorMessages.Add("La dirección de correo electrónico no puede tener mas de 50 caracteres");
                else
                {
                    User? userDb = await this.userRepository.GetUserByEmailAsync(user.Email);

                    if (userDb != null && (user.Id == 0 || userDb.Id != user.Id))
                        validationResultDTO.ErrorMessages.Add("El email debe ser único");
                }
            }

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int userId, bool isActive)
        {
            ValidationResultDTO resultDTO = new ValidationResultDTO();

            try
            {
                User obj = new User()
                {
                    Id = userId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.userRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                resultDTO.ErrorMessages.Add(ex.Message);
            }
            return resultDTO;
        }
    }
}
