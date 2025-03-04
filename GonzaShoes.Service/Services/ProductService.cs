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

            //if (string.IsNullOrWhiteSpace(Product.Name))
            //    validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre del usuario");
            //else if (Product.Name.Length > 50)
            //    validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");
            //else
            //{
            //    Product? ProductDb = await this.productRepository.GetProductByNameAsync(Product.Name);

            //    if (ProductDb != null && (Product.Id == 0 || ProductDb.Id != Product.Id))
            //        validationResultDTO.ErrorMessages.Add("El nombre de la marca debe ser único");
            //}

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
    }
}
