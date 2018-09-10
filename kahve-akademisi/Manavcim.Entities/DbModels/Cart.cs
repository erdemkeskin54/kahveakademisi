using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KahveAkademisi.Entities.DbModels
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductAmountType ProductAmountType { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; } = null;

        public DateTime AddToCartDate { get; set; }

        public CartRES CartDTtoRES()
        {
            CartRES cartRES = new CartRES()
            {
                Id = Id,
                AddToCartDate = AddToCartDate,
                Quantity = Quantity,
                ProductAmountType = ProductAmountType.ProductAmountTypeDTtoRES2()
            };

            return cartRES;
        }
    }
}
