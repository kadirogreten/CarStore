using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain
{
    public enum BodyType : byte
    {
        [Description("Hatchback")]
        Hatchback = 0,
        [Description("Sedan")]
        Sedan = 1,
        [Description("Wagon")]
        Wagon = 2,
        [Description("Coupe")]
        Coupe = 3,
        [Description("SUV")]
        SUV = 4

    }

    public enum FuelType : byte
    {
        [Description("Benzin")]
        Gasoline = 0,
        [Description("Gaz ve LPG")]
        GasLPG = 1,
        [Description("Dizel")]
        Diesel = 2,
        [Description("Hibrid")]
        Hybrid = 3,
        [Description("Elektrik")]
        Electric = 4
    }


    public enum GearType : byte
    {
        [Description("Manuel")]
        Manuel = 0,
        [Description("Automatic")]
        Automatic = 1,
        
    }

    public enum OrderStatus : byte
    {
        [Description("Tamamlandı")]
        Completed = 0,
        [Description("Beklemede")]
        Pending = 1,
    }

}
