using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectOnsMagasin;

public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(e => e.Description)
                  .HasMaxLength(250)
                  .IsRequired();

            entity.Property(e => e.Brand)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(e => e.ImagePath)
                  .IsRequired();

            entity.HasMany(e => e.ordersProducts)
                  .WithOne(e => e.Product)
                  .HasForeignKey(e => e.ProductId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.HasMany(e => e.Products)
                  .WithOne(e => e.Category)
                  .HasForeignKey(e => e.CategoryId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasMany(e => e.OrdersProducts)
                  .WithOne(e => e.Order)
                  .HasForeignKey(e => e.OrderId);
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.OrderId });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasIndex(e => e.Email)
                  .IsUnique();

            entity.Property(e => e.UserName)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(e => e.Address)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(e => e.City)
                  .IsRequired()
                  .HasMaxLength(150);

            entity.Property(e => e.PhoneNumber)
                  .IsRequired()
                  .HasMaxLength(25);

            entity.HasMany(e => e.Orders)
                  .WithOne(e => e.User)
                  .HasForeignKey(e => e.UserId);
        });
    }
}
