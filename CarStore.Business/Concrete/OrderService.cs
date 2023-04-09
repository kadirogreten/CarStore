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
    internal class OrderService : GenericRepository<Order, CarStoreDbContext>, IOrderService
    {
        public OrderService(CarStoreDbContext context) : base(context)
        {
        }

        public CarStoreDbContext Context { get { return _context as CarStoreDbContext; } }
    }
}