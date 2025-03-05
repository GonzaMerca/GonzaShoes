using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class BrandService : BaseService, IBrandService
    {
        private readonly IBrandRepository brandRepository;
        private readonly IModelProductService modelProductService;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public BrandService(IBrandRepository brandRepository, IModelProductService modelProductService, IProductService productService, IMapper mapper)
        {
            this.brandRepository = brandRepository;
            this.modelProductService = modelProductService;
            this.productService = productService;

            this.mapper = mapper;
        }

        public async Task<BrandDTO?> GetBrandByIdAsync(int id)
        {
            return this.mapper.Map<Brand, BrandDTO>(await this.brandRepository.GetBrandByIdAsync(id));
        }

        public async Task<BrandDTO?> GetBrandByNameAsync(string name)
        {
            return this.mapper.Map<Brand, BrandDTO>(await this.brandRepository.GetBrandByNameAsync(name));
        }

        public async Task<List<BrandDTO>> GetBrandsAsync(BrandSearchDTO searchDTO)
        {
            return this.mapper.Map<List<Brand>, List<BrandDTO>>(await this.brandRepository.GetBrandsAsync(searchDTO));
        }

        public async Task<List<NameIdDTO>> GetNameIdDTOsAsync()
        {
            return await this.brandRepository.GetNameIdDTOsAsync();
        }

        public async Task<ValidationResultDTO> SaveBrandAsync(BrandDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveBrandAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            Brand? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<BrandDTO, Brand>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.brandRepository.GetBrandByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.brandRepository.SaveBrandAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveBrandAsync(BrandDTO Brand)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(Brand.Name))
                validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre de la marca");
            else if (Brand.Name.Length > 50)
                validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");
            else
            {
                Brand? BrandDb = await this.brandRepository.GetBrandByNameAsync(Brand.Name);

                if (BrandDb != null && (Brand.Id == 0 || BrandDb.Id != Brand.Id))
                    validationResultDTO.ErrorMessages.Add("El nombre de la marca debe ser único");
            }

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int brandId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = await ValidateUpdateStatusAsync(brandId, isActive);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                Brand obj = new Brand()
                {
                    Id = brandId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.brandRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateUpdateStatusAsync(int brandId, bool isActive)
        {
            ValidationResultDTO resultDTO = new ValidationResultDTO();

            if (isActive && await this.modelProductService.IsAnyModelProductByBrandAsync(brandId))
                resultDTO.ErrorMessages.Add("La marca contiene modelos asociados, no puede anularlo");
            else if (isActive && await this.productService.IsAnyProductByBrandAsync(brandId))
                resultDTO.ErrorMessages.Add("La marca contiene productos asociados, no puede anularlo");

            return resultDTO;
        }
    }
}
