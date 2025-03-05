using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext dbContext;

        public BrandRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await this.dbContext.Brands.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Brand?> GetBrandByNameAsync(string name)
        {
            return await this.dbContext.Brands.SingleOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Brand>> GetBrandsAsync(BrandSearchDTO searchDTO)
        {
            IQueryable<Brand> query = this.dbContext.Brands;

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.Name.Contains(searchDTO.Name));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            return await query.ToListAsync();
        }

        public async Task<List<NameIdDTO>> GetNameIdDTOsAsync()
        {
            IQueryable<Brand> query = this.dbContext.Brands.Where(p => p.IsActive);

            return await query.Select(p => new NameIdDTO()
            {
                Id = p.Id,
                Name = p.Name,
                NameIds = p.ModelProducts.Where(x => x.IsActive)
                .Select(q => new NameIdDTO()
                {
                    Id = q.Id,
                    Name = q.Name,
                }).ToList()
            }).ToListAsync();
        }

        public async Task SaveBrandAsync(Brand obj)
        {
            if (obj.Id == 0)
                await this.dbContext.Brands.AddAsync(obj);
            else
                this.dbContext.Brands.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Brand obj)
        {
            this.dbContext.Brands.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
