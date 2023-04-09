using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain
{
    public class BaseEntitiy
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "ADMIN";
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Deleted { get; set; }
        public string? DeletedBy { get; set;}
    }
}
