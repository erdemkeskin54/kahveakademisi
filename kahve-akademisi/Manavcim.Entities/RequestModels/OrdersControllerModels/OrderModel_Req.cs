using KahveAkademisi.Entities.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.RequestModels.OrdersControllerModels
{
    public class OrderModel_Req
    {
        public List<OrderDetailModel_Req> OrderDetails { get; set; }
        public int UserAddressId { get; set; }
    }
}
