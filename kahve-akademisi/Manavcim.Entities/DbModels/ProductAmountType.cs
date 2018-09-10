
using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.DbModels
{
    public class ProductAmountType
    {
        public int Id { get; set; }
        public AmountType AmountType { get; set; }
        public Product Product { get; set; }

        public bool Choice { get; set; }
        public double Price { get; set; }
        public double Weight {get;set;}

        public double Stock { get; set; }

        public string CreateUserId { get; set; }
        [ForeignKey("CreateUserId")]
        public AppUser CreateUser { get; set; } = null;

        public string UpdateUserId { get; set; }
        [ForeignKey("UpdateUserId")]
        public AppUser UpdateUser { get; set; } = null;


        public ProductAmountTypeRES ProductAmountTypeDTtoRES()
        {
            ProductAmountTypeRES productAmountTypeRES = new ProductAmountTypeRES()
            {
                Id = Id,
                AmountType=AmountType,
                CreateUser=CreateUser!=null?CreateUser.AppUserDTtoRES():null,
                UpdateUser= UpdateUser != null ? UpdateUser.AppUserDTtoRES():null,
                Price=Price,
                Choice=Choice,
                Weight=Weight,
                Stock=Stock,

            };

            return productAmountTypeRES;
        }

        public ProductAmountTypeRES ProductAmountTypeDTtoRES2()
        {
            ProductAmountTypeRES productAmountTypeRES = new ProductAmountTypeRES()
            {
                Id = Id,
                AmountType = AmountType,
                CreateUser = CreateUser != null ? CreateUser.AppUserDTtoRES() : null,
                UpdateUser = UpdateUser != null ? UpdateUser.AppUserDTtoRES() : null,
                Price = Price,
                Choice = Choice,
                Weight = Weight,
                Stock = Stock,
                Product = Product != null ? Product.ProductDTtoRES2() : null

            };

            return productAmountTypeRES;
        }

    }
}
