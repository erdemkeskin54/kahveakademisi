using KahveAkademisi.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class OrderDetailRES
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ProductAmountTypeRES ProductAmountType { get; set; }
    }
}
