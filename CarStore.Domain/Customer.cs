using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain
{
    public class Customer : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "ADMIN";
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Deleted { get; set; }
        public string? DeletedBy { get; set; }


        public virtual ICollection<Car>? Cars { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
