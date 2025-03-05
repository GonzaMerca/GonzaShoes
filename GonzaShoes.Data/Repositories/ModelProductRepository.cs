using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class ModelProductRepository : IModelProductRepository
    {
        private readonly AppDbContext dbContext;

        public ModelProductRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ModelProduct?> GetModelProductByIdAsync(int id)
        {
            return await this.dbContext.ModelProducts.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ModelProduct?> GetModelProductByNameAsync(string name)
        {
            return await this.dbContext.ModelProducts.SingleOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<ModelProduct>> GetModelProductsAsync(ModelProductSearchDTO searchDTO)
        {
            IQueryable<ModelProduct> query = this.dbContext.ModelProducts.Include(x => x.Brand);

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.Name.Contains(searchDTO.Name));

            if (searchDTO.BrandIds != null && searchDTO.BrandIds.Count > 0)
                query = query.Where(p => searchDTO.BrandIds.Contains(p.BrandId));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            return await query.ToListAsync();
        }

        public async Task<List<NameIdDTO>> GetNameIdDTOsAsync()
        {
            IQueryable<ModelProduct> query = this.dbContext.ModelProducts.Where(p => p.IsActive);

            return await query.Select(p => new NameIdDTO()
            {
                Id = p.Id,
                Name = p.Name
            }).ToListAsync();
        }

        public async Task SaveModelProductAsync(ModelProduct obj)
        {
            if (obj.Id == 0)
                await this.dbContext.ModelProducts.AddAsync(obj);
            else
                this.dbContext.ModelProducts.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(ModelProduct obj)
        {
            this.dbContext.ModelProducts.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsAnyModelProductByBrandAsync(int brandId)
        {
            return await this.dbContext.ModelProducts.AnyAsync(p => p.BrandId == brandId);
        }
    }
}
