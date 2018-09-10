using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.RequestModels.OrdersControllerModels
{
    public class OrderDetailModel_Req
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public AmountType AmountType { get; set; }
        [Required]
        public bool Cargo { get; set; }

        public ProductSize Size { get; set; }
    }
}
