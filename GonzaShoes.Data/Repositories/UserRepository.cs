using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.User;
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

        public async Task<List<User>> GetUsersAsync(UserSearchDTO searchDTO)
        {
            IQueryable<User> query = this.dbContext.Users;

            if (!string.IsNullOrWhiteSpace(searchDTO.Name))
                query = query.Where(p => p.Name.Contains(searchDTO.Name));

            if (!string.IsNullOrWhiteSpace(searchDTO.Email))
                query = query.Where(p => p.Email.Contains(searchDTO.Email));

            if (searchDTO.ActivationState != null)
                query = query.Where(p => p.IsActive == searchDTO.GetActivationState());

            return await query.ToListAsync();
        }

        public async Task<List<NameIdDTO>> GetNameIdDTOsAsync()
        {
            IQueryable<User> query = this.dbContext.Users.Where(p => p.IsActive);

            return await query.Select(p => new NameIdDTO()
            {
                Id = p.Id,
                Name = p.Name,
                Tag = p.Email
            }).ToListAsync();
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
