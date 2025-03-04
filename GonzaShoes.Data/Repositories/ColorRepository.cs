using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs.Color;
using GonzaShoes.Model.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class ColorRepository : IColorRepository
    {
        private readonly AppDbContext dbContext;

        public ColorRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Color?> GetColorByIdAsync(int id)
        {
            return await this.dbContext.Colors.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Color?> GetColorByNameAsync(string name)
        {
            return await this.dbContext.Colors.SingleOrDefaultAsync(p => p.Name == name);
        }

        public async Task<List<Color>> GetColorsAsync(ColorSearchDTO searchDTO)
        {
            IQueryable<Color> query = this.dbContext.Colors;

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.Name.Contains(searchDTO.Name));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            return await query.ToListAsync();
        }

        public async Task SaveColorAsync(Color obj)
        {
            if (obj.Id == 0)
                await this.dbContext.Colors.AddAsync(obj);
            else
                this.dbContext.Colors.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Color obj)
        {
            this.dbContext.Colors.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
