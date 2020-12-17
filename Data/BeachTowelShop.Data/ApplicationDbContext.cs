using BeachTowelShop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BeachTowelShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public  DbSet<Size> Sizes { get; set; }
        public DbSet<Order> Orders { get; set; }


        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<TextProperty> TextProperties { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    
       

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Needed for Identity models configuration
            base.OnModelCreating(builder);

            builder.Entity<ProductSize>()
    .HasKey(bc => new { bc.ProductId, bc.SizeId });
            builder.Entity<ProductSize>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductSizes)
                .HasForeignKey(bc => bc.ProductId);
            builder.Entity<ProductSize>()
                .HasOne(bc => bc.Size)
                .WithMany(c => c.ProductSizes)
                .HasForeignKey(bc => bc.SizeId);

            builder.Entity<ProductPicture>()
   .HasKey(bc => new { bc.ProductId, bc.PictureId });
            builder.Entity<ProductPicture>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductPictures)
                .HasForeignKey(bc => bc.ProductId);
            builder.Entity<ProductPicture>()
                .HasOne(bc => bc.Picture)
                .WithMany(c => c.ProductPictures)
                .HasForeignKey(bc => bc.PictureId);

            builder.Entity<ProductCategory>()
   .HasKey(bc => new { bc.ProductId, bc.CategoryId });
            builder.Entity<ProductCategory>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductCategories)
                .HasForeignKey(bc => bc.ProductId);
            builder.Entity<ProductCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(bc => bc.CategoryId);



        }
    }
}
