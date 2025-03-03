using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User?> GetUserByEmailAsync(string emailAddress)
        {
            return await this.dbContext.Users.SingleOrDefaultAsync(p => p.Email == emailAddress.ToLowerInvariant());
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await this.dbContext.Users.SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await this.dbContext.Users.ToListAsync();
        }

        public async Task SaveUserAsync(User obj)
        {
            if (obj.Id == 0)
                await this.dbContext.Users.AddAsync(obj);
            else
                this.dbContext.Users.Update(obj);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(User obj)
        {
            this.dbContext.Users.Attach(obj);

            this.dbContext.Entry(obj).Property(p => p.IsActive).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.DateUpdated).IsModified = true;
            this.dbContext.Entry(obj).Property(p => p.UpdatedUserId).IsModified = true;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
