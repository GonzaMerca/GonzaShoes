using GonzaShoes.Model.Entities.Order;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Model.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace GonzaShoes.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ModelProduct> ModelProducts { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public DbSet<Size> Sizes { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ProductStockFlow> ProductStockFlows { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(decimal))
                    {
                        property.SetPrecision(18);
                        property.SetScale(5);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
