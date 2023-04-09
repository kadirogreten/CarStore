using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Dto_s
{
    public class CarViewModel
    {
        public string CarName { get; set; }
        public int BrandId { get; set; } = 0;
        public int Year { get; set; } = 0;
        public BodyType? BodyType { get; set; }
        public FuelType? FuelType { get; set; }
        public GearType? GearType { get; set; }
        public string Color { get; set; } = "#FFF";
        public decimal? PriceWithTax { get; set; }
        
    }
}
