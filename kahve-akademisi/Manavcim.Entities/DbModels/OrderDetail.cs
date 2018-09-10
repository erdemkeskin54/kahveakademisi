using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.DbModels
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public double TotalPrice { get; set; }


        public int ProductAmountTypeId { get; set; }
        [ForeignKey("ProductAmountTypeId")]
        public ProductAmountType ProductAmountType { get; set; }

   
        public  Order Order { get; set; }

        public OrderDetailRES OrderDetailDTtoRES()
        {
            OrderDetailRES orderDetailRES = new OrderDetailRES()
            {
                Id=Id,
                OrderStatus=OrderStatus,
                Quantity= Quantity,
                ProductAmountType=ProductAmountType.ProductAmountTypeDTtoRES2(),
                TotalPrice=TotalPrice
            };

            return orderDetailRES;
        }
    }
}
