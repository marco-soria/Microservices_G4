using Microservices.Services.ShopingCartAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microservices.Services.ShopingCartAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
    }
}
