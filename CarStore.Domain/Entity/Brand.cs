using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain
{
    public class Brand : BaseEntitiy
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

    }
}
