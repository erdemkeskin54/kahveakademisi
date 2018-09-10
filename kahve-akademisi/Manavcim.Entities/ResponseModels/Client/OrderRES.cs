using System;
using System.Collections.Generic;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class OrderRES
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }


        public UserAdressRES UserAdress { get; set; }
        public List<OrderDetailRES> OrderDetails { get; set; }

    }
}
