using CarStore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Core
{
    public class SeedData
    {

        private readonly ModelBuilder modelBuilder;

        public SeedData(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            modelBuilder.Entity<Brand>().HasData(
                   new Brand() {
                       Id = 1,
                       Name = "AUDI",      
                       Description = "AUDI",
                       Created = DateTime.Now,
                       CreatedBy = "ADMIN"
                   },
                   new Brand()
                   {
                       Id = 2,
                       Name = "BMW",
                       Description = "BMW",
                       Created = DateTime.Now,
                       CreatedBy = "ADMIN"
                   },
                   new Brand()
                   {
                       Id = 3,
                       Name = "MERCEDES",
                       Description = "MERCEDES",
                       Created = DateTime.Now,
                       CreatedBy = "ADMIN"
                   },
                   new Brand()
                   {
                       Id = 4,
                       Name = "NISSAN",
                       Description = "NISSAN",
                       Created = DateTime.Now,
                       CreatedBy = "ADMIN"
                   },
                   new Brand()
                   {
                       Id = 5,
                       Name = "OPEL",
                       Description = "OPEL",
                       Created = DateTime.Now,
                       CreatedBy = "ADMIN"
                   },
                   new Brand()
                   {
                       Id = 6,
                       Name = "RENAULT",
                       Description = "RENAULT",
                       Created = DateTime.Now,
                       CreatedBy = "ADMIN"
                       
                   }
            );
           
        }

    }
}
