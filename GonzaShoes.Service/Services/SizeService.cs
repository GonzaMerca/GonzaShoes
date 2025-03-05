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
        private readonly IProductService productService;

        private readonly IMapper mapper;

        public SizeService(ISizeRepository sizeRepository, IProductService productService, IMapper mapper)
        {
            this.sizeRepository = sizeRepository;
            this.productService = productService;

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

        public async Task<List<NameIdDTO>> GetNameIdDTOsAsync()
        {
            return await this.sizeRepository.GetNameIdDTOsAsync();
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

            if (Size.Number <= 0)
                validationResultDTO.ErrorMessages.Add("Falta ingresar el número del talle");
            else
            {
                Size? SizeDb = await this.sizeRepository.GetSizeByNumberAsync(Size.Number);

                if (SizeDb != null && (Size.Id == 0 || SizeDb.Id != Size.Id))
                    validationResultDTO.ErrorMessages.Add("El número del talle debe ser único");
            }

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int SizeId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = await ValidateUpdateStatusAsync(SizeId, isActive);
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

        private async Task<ValidationResultDTO> ValidateUpdateStatusAsync(int colorId, bool isActive)
        {
            ValidationResultDTO resultDTO = new ValidationResultDTO();

            if (isActive && await this.productService.IsAnyProductBySizeAsync(colorId))
                resultDTO.ErrorMessages.Add("El talle contiene productos asociados, no puede anularlo");

            return resultDTO;
        }
    }
}
