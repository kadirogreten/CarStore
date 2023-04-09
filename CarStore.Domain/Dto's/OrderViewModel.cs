using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Dto_s
{
    public class OrderViewModel
    {
        public int CarId { get; set; } = 0;
        public string? CustomerId { get; set; }
        public string? SalerId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Installment { get; set; } = 1;
               
    }


    public class PaymentViewModel
    {
        public int OrderId { get; set; } = 0;
        public int CarId { get; set; } = 0;
        public string? CustomerId { get; set; }
        public string? SalerId { get; set; }
    }
}
