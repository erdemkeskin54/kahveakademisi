using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class CartRES
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductAmountTypeRES ProductAmountType { get; set; }
        public DateTime AddToCartDate { get; set; }
    }
}
