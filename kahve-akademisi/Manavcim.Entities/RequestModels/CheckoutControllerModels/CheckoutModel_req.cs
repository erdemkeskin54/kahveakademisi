
using KahveAkademisi.Entities.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.RequestModels.CheckoutControllerModels
{
    public class CheckoutModel_req
    {

        [Required]
        public int OldAddressId { get; set; }
        [RequiredIf("OldAddressId", 0)]
        public string Address { get; set; }
        [RequiredIf("OldAddressId", 0)]
        public string AddressTitle { get; set; }
        [RequiredIf("OldAddressId", 0)]
        public string City { get; set; }
        [RequiredIf("OldAddressId", 0)]
        public string District { get; set; }
        [RequiredIf("OldAddressId", 0)]
        public string Neighborhood { get; set; }
        [RequiredIf("OldAddressId", 0)]
        public string Zip { get; set; }
    
        public string DeliveryDate { get; set; }
  
        public string DeliveryTime { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [RequiredIf("PaymentMethod", PaymentMethod.CreditCard)]
        public int Installment { get; set; }
        [RequiredIf("PaymentMethod", PaymentMethod.CreditCard)]
        public string FullName { get; set; }
        [RequiredIf("PaymentMethod", PaymentMethod.CreditCard)]
        public string CardNumber { get; set; }
        [RequiredIf("PaymentMethod", PaymentMethod.CreditCard)]
        public string Cvc { get; set; }
        [RequiredIf("PaymentMethod", PaymentMethod.CreditCard)]
        public string Mounth { get; set; }
        [RequiredIf("PaymentMethod", PaymentMethod.CreditCard)]
        public string Year { get; set; }




    }
}
