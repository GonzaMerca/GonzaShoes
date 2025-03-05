using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;

            this.mapper = mapper;
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            return this.mapper.Map<Product, ProductDTO>(await this.productRepository.GetProductByIdAsync(id));
        }

        public async Task<ProductDTO?> GetProductByNameAsync(string name)
        {
            return this.mapper.Map<Product, ProductDTO>(await this.productRepository.GetProductByNameAsync(name));
        }

        public async Task<List<ProductDTO>> GetProductsAsync(ProductSearchDTO searchDTO)
        {
            return this.mapper.Map<List<Product>, List<ProductDTO>>(await this.productRepository.GetProductsAsync(searchDTO));
        }

        public async Task<ValidationResultDTO> SaveProductAsync(ProductDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveProductAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            Product? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<ProductDTO, Product>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.productRepository.GetProductByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.productRepository.SaveProductAsync(obj);

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveProductAsync(ProductDTO Product)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            if (Product.Price < 0)
                validationResultDTO.ErrorMessages.Add("El precio del producto no puede ser negativo");
            if (Product.Stock < 0)
                validationResultDTO.ErrorMessages.Add("El stock del producto no puede ser negativo");
            if (await this.productRepository.IsAnyProductAsync(Product))
                validationResultDTO.ErrorMessages.Add("Ya existe un producto con estas caracteristiscas");

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int ProductId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                Product obj = new Product()
                {
                    Id = ProductId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.productRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }

        public async Task<bool> IsAnyProductByBrandAsync(int brandId)
        {
            return await productRepository.IsAnyProductByBrandAsync(brandId);
        }

        public async Task<bool> IsAnyProductByModelProductAsync(int modelProduct)
        {
            return await productRepository.IsAnyProductByModelProductAsync(modelProduct);
        }

        public async Task<bool> IsAnyProductByColorAsync(int colorId)
        {
            return await productRepository.IsAnyProductByColorAsync(colorId);
        }

        public async Task<bool> IsAnyProductBySizeAsync(int sizeId)
        {
            return await productRepository.IsAnyProductBySizeAsync(sizeId);
        }
    }
}
