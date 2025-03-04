using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Color;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class ColorService : BaseService, IColorService
    {
        private readonly IColorRepository colorRepository;
        private readonly IMapper mapper;

        public ColorService(IColorRepository colorRepository, IMapper mapper)
        {
            this.colorRepository = colorRepository;

            this.mapper = mapper;
        }

        public async Task<ColorDTO?> GetColorByIdAsync(int id)
        {
            return this.mapper.Map<Color, ColorDTO>(await this.colorRepository.GetColorByIdAsync(id));
        }

        public async Task<ColorDTO?> GetColorByNameAsync(string name)
        {
            return this.mapper.Map<Color, ColorDTO>(await this.colorRepository.GetColorByNameAsync(name));
        }

        public async Task<List<ColorDTO>> GetColorsAsync(ColorSearchDTO searchDTO)
        {
            return this.mapper.Map<List<Color>, List<ColorDTO>>(await this.colorRepository.GetColorsAsync(searchDTO));
        }

        public async Task<ValidationResultDTO> SaveColorAsync(ColorDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveColorAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            Color? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<ColorDTO, Color>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.colorRepository.GetColorByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.colorRepository.SaveColorAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveColorAsync(ColorDTO Color)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(Color.Name))
                validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre del usuario");
            else if (Color.Name.Length > 50)
                validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");
            else
            {
                Color? ColorDb = await this.colorRepository.GetColorByNameAsync(Color.Name);

                if (ColorDb != null && (Color.Id == 0 || ColorDb.Id != Color.Id))
                    validationResultDTO.ErrorMessages.Add("El nombre de la marca debe ser único");
            }

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int ColorId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                Color obj = new Color()
                {
                    Id = ColorId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.colorRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }
    }
}
