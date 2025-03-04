using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs.Order;
using GonzaShoes.Model.Entities.Order;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await this.dbContext.Orders.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Order>> GetOrdersAsync(OrderSearchDTO searchDTO)
        {
            IQueryable<Order> query = this.dbContext.Orders;

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            return await query.ToListAsync();
        }

        public async Task SaveOrderAsync(Order obj)
        {
            if (obj.Id == 0)
                await this.dbContext.Orders.AddAsync(obj);
            else
                this.dbContext.Orders.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Order obj)
        {
            this.dbContext.Orders.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
