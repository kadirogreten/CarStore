using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Response
{
    public class CarListResponseModel
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int Year { get; set; }
        public BodyType BodyType { get; set; }
        public FuelType FuelType { get; set; }
        public GearType GearType { get; set; }
        public string Color { get; set; } = "#FFF";
        public decimal PriceWithTax { get; set; }
        public DateTime? SalesDate { get; set; }
        public BuyerResponseModel? Buyer { get; set; }

        public SalerResponseModel Saler { get; set; }
    }



    public class SalerResponseModel
    {
        public string SalerId { get; set; }
        public string SalerName { get; set; }
        public string SalerSurname { get; set; }
    }

    public class BuyerResponseModel
    {
        public string? BuyerId { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerSurname { get; set; }
    }
}
