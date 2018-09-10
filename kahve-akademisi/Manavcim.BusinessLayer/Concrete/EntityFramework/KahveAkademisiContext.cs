using KahveAkademisi.Entities.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.BusinessLayer.Concrete.EntityFramework
{
    public class KahveAkademisiContext : IdentityDbContext<AppUser>
    {
        public KahveAkademisiContext(DbContextOptions<KahveAkademisiContext> options)
            : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImageGallery> ProductImageGalleries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UserAddress> UserAdresses { get; set; }
        public DbSet<ProductAmountType> ProductAmountTypes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<PaymentResult> PaymentResults { get; set; }
        public DbSet<PaymentItem> PaymentItems { get; set; }
        public DbSet<ConvertedPayout> ConvertedPayouts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
