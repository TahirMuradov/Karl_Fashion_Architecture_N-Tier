using Entities;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.SQLserver
{
    public class AppDbContext:IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = localhost; Database = KarlFashionAppDb; Trusted_Connection = True; MultipleActiveResultSets = True; TrustServerCertificate = True;");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Order> Orders {  get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodLaunge> PaymentMethodsLaunge { get; set; }

        public DbSet<Item> SoldProducts { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<ProductLanguages> ProductLanguages { get; set; }


        public DbSet<Size> Sizes { get; set; }
     
     
        public DbSet<ProductSize> ProductSizes { get; set; }

        public DbSet<ShippingMethods>ShippingMethods { get; set; }
        public DbSet<ShippingLaunguage> ShippingLaunguages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
           
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            modelBuilder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(y => y.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>()
                    .HasOne(x => x.User)
                    .WithMany(y => y.Products)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(deleteBehavior: DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductSize>()
           .HasOne(x => x.Size)
           .WithMany(y => y.ProductSize)
           .HasForeignKey(x => x.SizeId)
           .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
            modelBuilder.Entity<Size>()
                            .HasOne(x => x.User)
                            .WithMany(y => y.Sizes)
                            .HasForeignKey(x => x.UserId)
                            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PaymentMethodLaunge>()
                     .HasOne(x => x.PaymentMethod)
                     .WithMany(y => y.PaymentMethodLaunguages)
                     .HasForeignKey(x => x.PaymentMehtodId)
                     .OnDelete(DeleteBehavior.Restrict);
                
                
       

        }



    }
}
