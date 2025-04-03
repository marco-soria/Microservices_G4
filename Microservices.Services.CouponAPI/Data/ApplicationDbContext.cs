using Microservices.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.CouponAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupon { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon 
            { 
                CouponId = 1, 
                CouponCode = "10OFF", 
                DiscountAmount = 10, 
                MinimumAmount = 20 
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon 
            { 
                CouponId = 2, 
                CouponCode = "20OFF", 
                DiscountAmount = 20, 
                MinimumAmount = 40
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponId = 3,
                CouponCode = "30OFF",
                DiscountAmount = 30,
                MinimumAmount = 60
            });
        }
    }
}
