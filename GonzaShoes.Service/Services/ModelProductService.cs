using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class ModelProductService : BaseService, IModelProductService
    {
        private readonly IModelProductRepository modelProductRepository;
        private readonly IMapper mapper;

        public ModelProductService(IModelProductRepository modelProductRepository, IMapper mapper)
        {
            this.modelProductRepository = modelProductRepository;

            this.mapper = mapper;
        }

        public async Task<ModelProductDTO?> GetModelProductByIdAsync(int id)
        {
            return this.mapper.Map<ModelProduct, ModelProductDTO>(await this.modelProductRepository.GetModelProductByIdAsync(id));
        }

        public async Task<ModelProductDTO?> GetModelProductByNameAsync(string name)
        {
            return this.mapper.Map<ModelProduct, ModelProductDTO>(await this.modelProductRepository.GetModelProductByNameAsync(name));
        }

        public async Task<List<ModelProductDTO>> GetModelProductsAsync(ModelProductSearchDTO searchDTO)
        {
            return this.mapper.Map<List<ModelProduct>, List<ModelProductDTO>>(await this.modelProductRepository.GetModelProductsAsync(searchDTO));
        }

        public async Task<ValidationResultDTO> SaveModelProductAsync(ModelProductDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveModelProductAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            ModelProduct? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<ModelProductDTO, ModelProduct>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.modelProductRepository.GetModelProductByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.modelProductRepository.SaveModelProductAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveModelProductAsync(ModelProductDTO ModelProduct)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            if (string.IsNullOrWhiteSpace(ModelProduct.Name))
                validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre del usuario");
            else if (ModelProduct.Name.Length > 50)
                validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");
            else
            {
                ModelProduct? ModelProductDb = await this.modelProductRepository.GetModelProductByNameAsync(ModelProduct.Name);

                if (ModelProductDb != null && (ModelProduct.Id == 0 || ModelProductDb.Id != ModelProduct.Id))
                    validationResultDTO.ErrorMessages.Add("El nombre de la marca debe ser único");
            }

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int ModelProductId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                ModelProduct obj = new ModelProduct()
                {
                    Id = ModelProductId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.modelProductRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }
    }
}
