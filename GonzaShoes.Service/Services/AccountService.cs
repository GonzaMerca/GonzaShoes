using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.Configurations;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.User;
using GonzaShoes.Model.Entities.User;
using GonzaShoes.Model.Helper;
using GonzaShoes.Service.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GonzaShoes.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository userRepository;
        private readonly IOptions<AppConfiguration> appConfiguration;

        public AccountService(IUserRepository userRepository, IOptions<AppConfiguration> appConfiguration)
        {
            this.userRepository = userRepository;
            this.appConfiguration = appConfiguration;
        }

        public async Task<LoginResultDTO> LoginUserAsync(string email, string password)
        {
            LoginResultDTO loginResultDTO = new LoginResultDTO();

            try
            {
                User? user = await this.userRepository.GetUserByEmailAsync(email.Trim());

                if (user == null || !SecurePasswordHasher.Verify(password, user.Password))
                {
                    loginResultDTO.ValidationResult.ErrorMessages.Add("Usuario o contraseña incorrectos");
                    return loginResultDTO;
                }

                if (user != null && !user.IsActive)
                {
                    loginResultDTO.ValidationResult.ErrorMessages.Add("El usuario se encuentra anulado");
                    return loginResultDTO;
                }

                loginResultDTO.Token = BuildToken(user);
            }
            catch (Exception ex)
            {
                loginResultDTO.ValidationResult.ErrorMessages.Add(ex.Message);
            }

            return loginResultDTO;
        }

        private string BuildToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.Value.JWTSecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName", user.Name),
                new Claim("Email", user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: "GonzaShoes",
                audience: "GonzaShoes",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
