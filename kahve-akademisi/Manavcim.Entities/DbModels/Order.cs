using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.DbModels
{
    public class Order
    {
        public int Id{ get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public double  TotalPrice { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        public int UserAddressId { get; set; }
        [ForeignKey("UserAddressId")]
        public UserAddress UserAdress { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }


        public OrderRES OrderDTtoRES()
        {
            OrderRES orderRES = new OrderRES()
            {
                 Id=Id,
                 OrderDate=OrderDate,
                 OrderDetails=OrderDetails.Select(x=>x.OrderDetailDTtoRES()).ToList(),
                 OrderStatus=OrderStatus,
                 UserAdress=UserAdress.UserAdressDTtoRES()

            };

            return orderRES;
        }

    }
}
