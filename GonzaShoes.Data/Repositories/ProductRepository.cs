using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await this.dbContext.Products.Include(x => x.ModelProduct)
                                                .Include(x => x.Brand)
                                                .Include(x => x.Color)
                                                .Include(x => x.Size)
                                                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await this.dbContext.Products.SingleOrDefaultAsync(p => p.ModelProduct.Name == name);
        }

        public async Task<List<Product>> GetProductsAsync(ProductSearchDTO searchDTO)
        {
            IQueryable<Product> query = this.dbContext.Products.Include(x => x.ModelProduct)
                                                               .Include(x => x.Brand)
                                                               .Include(x => x.Color)
                                                               .Include(x => x.Size);

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.ModelProduct.Name.Contains(searchDTO.Name));

            if (searchDTO.BrandIds != null && searchDTO.BrandIds.Count > 0)
                query = query.Where(p => searchDTO.BrandIds.Contains(p.BrandId));

            if (searchDTO.ModelProductIds != null && searchDTO.ModelProductIds.Count > 0)
                query = query.Where(p => searchDTO.ModelProductIds.Contains(p.ModelProductId));

            if (searchDTO.ColorIds != null && searchDTO.ColorIds.Count > 0)
                query = query.Where(p => searchDTO.ColorIds.Contains(p.ColorId));

            if (searchDTO.SizeIds != null && searchDTO.SizeIds.Count > 0)
                query = query.Where(p => searchDTO.SizeIds.Contains(p.SizeId));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            if (searchDTO.OnlyWithStock)
                query = query.Where(p => p.Stock > 0);

            return await query.ToListAsync();
        }

        public async Task SaveProductAsync(Product obj)
        {
            if (obj.Id == 0)
                await this.dbContext.Products.AddAsync(obj);
            else
                this.dbContext.Products.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Product obj)
        {
            this.dbContext.Products.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> ValidateStockAsync(int productId, int quantity)
        {
            return await this.dbContext.Products.AnyAsync(p => p.Id == productId && (p.Stock - quantity) >= 0);
        }

        public async Task<bool> IsAnyProductAsync(ProductDTO product)
        {
            return await this.dbContext.Products.AnyAsync(p => p.Id != p.Id && p.BrandId == product.BrandId && p.ModelProductId == product.ModelProductId &&
                                                               p.ColorId == product.ColorId && p.SizeId == product.SizeId);
        }

        public async Task<bool> IsAnyProductByBrandAsync(int brandId)
        {
            return await this.dbContext.Products.AnyAsync(p => p.BrandId == brandId);
        }

        public async Task<bool> IsAnyProductByModelProductAsync(int modelProduct)
        {
            return await this.dbContext.Products.AnyAsync(p => p.ModelProductId == modelProduct);
        }

        public async Task<bool> IsAnyProductByColorAsync(int colorId)
        {
            return await this.dbContext.Products.AnyAsync(p => p.ColorId == colorId);
        }

        public async Task<bool> IsAnyProductBySizeAsync(int sizeId)
        {
            return await this.dbContext.Products.AnyAsync(p => p.SizeId == sizeId);
        }
    }
}
