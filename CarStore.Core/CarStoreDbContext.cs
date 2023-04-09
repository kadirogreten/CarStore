using CarStore.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarStore.Core
{
    public class CarStoreDbContext : IdentityDbContext<Customer>
    {
        public CarStoreDbContext(DbContextOptions<CarStoreDbContext> options)
           : base(options)
        {

        }

        public DbSet<Car> Car { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Order> Order { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new SeedData(builder).Seed();
        }

    }
}