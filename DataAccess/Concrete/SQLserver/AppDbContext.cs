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
            optionsBuilder.UseSqlServer("Server = DESKTOP-EQA4SQR; Database = KarlFashionAppDb; Trusted_Connection = True; MultipleActiveResultSets = True; TrustServerCertificate = True;");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLanguage> CategoryLanguages { get; set; }
        public DbSet<Order> Orders {  get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product>Products { get; set; }
        public DbSet<ProductLanguages> ProductLanguages { get; set; }
     public DbSet<UserActions> Actions { get; set; }
        public DbSet<Size> Sizes { get; set; }
       
        public DbSet<ProductSize> ProductSizes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
           
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            modelBuilder.Entity<Order>()
                .HasOne(x => x.User)
                .WithMany(y=>y.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
         

        }



    }
}
