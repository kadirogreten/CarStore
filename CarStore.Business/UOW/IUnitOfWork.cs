using CarStore.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Business.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public ICarService Car { get; }
        public ICustomerService Customer { get; }
        public IBrandService Brand { get; }
        public IOrderService Order { get; }


        Task<int> SaveChanges();

    }
}
