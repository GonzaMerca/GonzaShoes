using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs.Size;
using GonzaShoes.Model.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class SizeRepository : ISizeRepository
    {
        private readonly AppDbContext dbContext;

        public SizeRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Size?> GetSizeByIdAsync(int id)
        {
            return await this.dbContext.Sizes.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Size?> GetSizeByNumberAsync(decimal number)
        {
            return await this.dbContext.Sizes.SingleOrDefaultAsync(p => p.Number == number);
        }

        public async Task<List<Size>> GetSizesAsync(SizeSearchDTO searchDTO)
        {
            IQueryable<Size> query = this.dbContext.Sizes;

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.Number.ToString().Contains(searchDTO.Name));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            return await query.ToListAsync();
        }

        public async Task SaveSizeAsync(Size obj)
        {
            if (obj.Id == 0)
                await this.dbContext.Sizes.AddAsync(obj);
            else
                this.dbContext.Sizes.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Size obj)
        {
            this.dbContext.Sizes.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
