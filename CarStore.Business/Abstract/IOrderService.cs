using CarStore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Business.Abstract
{
    public interface IOrderService : IRepository<Order>
    {
    }
}
