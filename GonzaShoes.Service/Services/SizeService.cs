using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Size;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class SizeService : BaseService, ISizeService
    {
        private readonly ISizeRepository sizeRepository;
        private readonly IMapper mapper;

        public SizeService(ISizeRepository sizeRepository, IMapper mapper)
        {
            this.sizeRepository = sizeRepository;

            this.mapper = mapper;
        }

        public async Task<SizeDTO?> GetSizeByIdAsync(int id)
        {
            return this.mapper.Map<Size, SizeDTO>(await this.sizeRepository.GetSizeByIdAsync(id));
        }

        public async Task<SizeDTO?> GetSizeByNumberAsync(decimal number)
        {
            return this.mapper.Map<Size, SizeDTO>(await this.sizeRepository.GetSizeByNumberAsync(number));
        }

        public async Task<List<SizeDTO>> GetSizesAsync(SizeSearchDTO searchDTO)
        {
            return this.mapper.Map<List<Size>, List<SizeDTO>>(await this.sizeRepository.GetSizesAsync(searchDTO));
        }

        public async Task<ValidationResultDTO> SaveSizeAsync(SizeDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveSizeAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            Size? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<SizeDTO, Size>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.sizeRepository.GetSizeByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.sizeRepository.SaveSizeAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveSizeAsync(SizeDTO Size)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            //if (string.IsNullOrWhiteSpace(Size.Name))
            //    validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre del usuario");
            //else if (Size.Name.Length > 50)
            //    validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");
            //else
            //{
            //    Size? SizeDb = await this.sizeRepository.GetSizeByNameAsync(Size.Name);

            //    if (SizeDb != null && (Size.Id == 0 || SizeDb.Id != Size.Id))
            //        validationResultDTO.ErrorMessages.Add("El nombre de la marca debe ser único");
            //}

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int SizeId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                Size obj = new Size()
                {
                    Id = SizeId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.sizeRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }
    }
}
