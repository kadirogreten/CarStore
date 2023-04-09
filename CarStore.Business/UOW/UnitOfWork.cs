using CarStore.Business.Abstract;
using CarStore.Business.Concrete;
using CarStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Business.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarStoreDbContext _db;
        public UnitOfWork(CarStoreDbContext db)
        {
            _db = db;

            Car = new CarService(_db);
            Customer = new CustomerService(_db);
            Brand = new BrandService(_db);
            Order = new OrderService(_db);

        }

        public ICarService Car {get; private set;}

        public ICustomerService Customer { get; private set; }

        public IBrandService Brand { get; private set; }

        public IOrderService Order { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<int> SaveChanges()
        {
           return await _db.SaveChangesAsync();
        }
    }
}
