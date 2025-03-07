using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class ProductStockFlowRepository : IProductStockFlowRepository
    {
        private readonly AppDbContext dbContext;

        public ProductStockFlowRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ProductStockFlow?> GetProductStockFlowByIdAsync(int id)
        {
            return await this.dbContext.ProductStockFlows.Include(x => x.ModelProduct)
                                                         .Include(x => x.Brand)
                                                         .Include(x => x.Color)
                                                         .Include(x => x.Size)
                                                         .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductStockFlow>> GetProductStockFlowsAsync()
        {
            IQueryable<ProductStockFlow> query = this.dbContext.ProductStockFlows.Include(x => x.ModelProduct)
                                                                                 .Include(x => x.Brand)
                                                                                 .Include(x => x.Color)
                                                                                 .Include(x => x.Size);

            return await query.ToListAsync();
        }

        public async Task SaveProductStockFlowsAsync(ProductStockFlow obj)
        {
            if (obj.Id == 0)
                await this.dbContext.ProductStockFlows.AddAsync(obj);
            else
                this.dbContext.ProductStockFlows.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(ProductStockFlow obj)
        {
            this.dbContext.ProductStockFlows.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
