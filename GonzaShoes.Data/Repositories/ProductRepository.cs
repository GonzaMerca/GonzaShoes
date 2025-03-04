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
            return await this.dbContext.Products.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await this.dbContext.Products.SingleOrDefaultAsync(p => p.Model.Name == name);
        }

        public async Task<List<Product>> GetProductsAsync(ProductSearchDTO searchDTO)
        {
            IQueryable<Product> query = this.dbContext.Products.Include(x => x.Model);

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.Model.Name.Contains(searchDTO.Name));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

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
    }
}
