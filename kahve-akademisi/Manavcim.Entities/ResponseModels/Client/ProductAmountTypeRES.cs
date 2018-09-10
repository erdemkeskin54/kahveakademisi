using System;
using System.Collections.Generic;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class ProductAmountTypeRES
    {
        public int Id { get; set; }
        public AmountType AmountType { get; set; }
        public double Price { get; set; }

        public bool Choice { get; set; }
        public double Weight { get; set; }

        public double Stock { get; set; }
        public ProductRES Product {get;set;}

        public AppUserRES CreateUser { get; set; }
        public AppUserRES UpdateUser { get; set; }
    }
}
