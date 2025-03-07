using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model;
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
            return await this.dbContext.Orders.Include(x => x.OrderPayment)
                                              .Include(x => x.OrderItems)
                                               .ThenInclude(x => x.Product)
                                              .Include(x => x.OrderItems)
                                               .ThenInclude(x => x.Brand)
                                              .Include(x => x.OrderItems)
                                               .ThenInclude(x => x.ModelProduct)
                                              .Include(x => x.OrderItems)
                                               .ThenInclude(x => x.Color)
                                              .Include(x => x.OrderItems)
                                               .ThenInclude(x => x.Size)
                                              .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Order>> GetOrdersAsync(OrderSearchDTO dto)
        {
            IQueryable<Order> query = this.dbContext.Orders.Include(x => x.OrderPayment)
                                                           .Include(x => x.OrderItems)
                                                            .ThenInclude(x => x.Product)
                                                           .Include(x => x.OrderItems)
                                                            .ThenInclude(x => x.Brand)
                                                           .Include(x => x.OrderItems)
                                                            .ThenInclude(x => x.ModelProduct)
                                                           .Include(x => x.OrderItems)
                                                            .ThenInclude(x => x.Color)
                                                           .Include(x => x.OrderItems)
                                                            .ThenInclude(x => x.Size);

            if (dto.OrderId != null && dto.OrderId > 0)
            {
                query = query.Where(p => p.Id == dto.OrderId);
            }

            if (dto.UserId != null && dto.UserId > 0)
                query = query.Where(p => p.CreatedUserId == dto.UserId);

            if (dto.PaymentMethodId != null && dto.PaymentMethodId > 0)
            {
                query = query.Where(p => (dto.PaymentMethodId == (int)OrderPaymentTypeEnum.Cash && p.OrderPayment != null && p.OrderPayment.Cash > 0) ||
                                         (dto.PaymentMethodId == (int)OrderPaymentTypeEnum.Debit && p.OrderPayment != null && p.OrderPayment.DebitCard > 0) ||
                                         (dto.PaymentMethodId == (int)OrderPaymentTypeEnum.Credit && p.OrderPayment != null && p.OrderPayment.CreditCard > 0) ||
                                         (dto.PaymentMethodId == (int)OrderPaymentTypeEnum.Transfer && p.OrderPayment != null && p.OrderPayment.Transfer > 0));
            }

            if (dto.ActivationState != null)
                query = query.Where(p => p.IsActive == dto.GetActivationState());

            return await query.Where(p => p.DateTime.Date >= dto.DateFrom.Value.Date && p.DateTime.Date <= dto.DateTo.Value.Date).ToListAsync();
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
