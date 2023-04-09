using CarStore.Business.Abstract;
using CarStore.Core;
using CarStore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Business.Concrete
{
    public class CarService : GenericRepository<Car, CarStoreDbContext>, ICarService
    {
        public CarService(CarStoreDbContext context) : base(context)
        {
        }

        public CarStoreDbContext Context { get { return _context as CarStoreDbContext; } }
    }
}
