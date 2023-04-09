using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain
{
    public class Order : BaseEntitiy
    {
        public int CarId { get; set; }
        public string CustomerId { get; set; }
        public string SalerId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Installment { get; set; } = 1;
        public int? PayedInstallment { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public DateTime? CompletedAt { get; set; }

        [ForeignKey(nameof(CarId))]
        public Car Car { get; set; }
    }
}
