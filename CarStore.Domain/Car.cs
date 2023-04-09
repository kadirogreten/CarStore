using System.ComponentModel.DataAnnotations.Schema;

namespace CarStore.Domain
{
    public class Car : BaseEntitiy
    {
        public string CarName { get; set; }
        public int BrandId { get; set; }
        public int Year { get; set; }
        public BodyType BodyType { get; set; }
        public FuelType FuelType { get; set; }
        public GearType GearType { get; set; }
        public string Color { get; set; } = "#FFF";
        public decimal PriceWithTax { get; set; }
        public DateTime? SalesDate { get; set; }
        public string? BuyerId { get; set; }
        public string SalerId { get; set; }


        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }
    }
}