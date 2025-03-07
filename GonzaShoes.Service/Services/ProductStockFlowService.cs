using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class ProductStockFlowService : BaseService, IProductStockFlowService
    {
        private readonly IProductStockFlowRepository productStockFlowRepository;
        private readonly IMapper mapper;

        public ProductStockFlowService(IProductStockFlowRepository productStockFlowRepository, IMapper mapper)
        {
            this.productStockFlowRepository = productStockFlowRepository;

            this.mapper = mapper;
        }

        public async Task<ProductStockFlowDTO?> GetProductStockFlowByIdAsync(int id)
        {
            return this.mapper.Map<ProductStockFlow, ProductStockFlowDTO>(await this.productStockFlowRepository.GetProductStockFlowByIdAsync(id));
        }

        public async Task<List<ProductStockFlowDTO>> GetProductStockFlowsAsync()
        {
            return this.mapper.Map<List<ProductStockFlow>, List<ProductStockFlowDTO>>(await this.productStockFlowRepository.GetProductStockFlowsAsync());
        }

        public async Task<ValidationResultDTO> SaveProductStockFlowsAsync(ProductStockFlowDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveProductStockFlowAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            ProductStockFlow? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<ProductStockFlowDTO, ProductStockFlow>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.productStockFlowRepository.GetProductStockFlowByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.productStockFlowRepository.SaveProductStockFlowsAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveProductStockFlowAsync(ProductStockFlowDTO Product)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int productStockFlowId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                ProductStockFlow obj = new ProductStockFlow()
                {
                    Id = productStockFlowId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.productStockFlowRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }
    }
}
