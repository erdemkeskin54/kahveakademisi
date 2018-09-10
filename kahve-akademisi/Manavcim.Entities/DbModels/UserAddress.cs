using KahveAkademisi.Entities.ResponseModels.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KahveAkademisi.Entities.DbModels
{
    public class UserAddress
    {
        [Key]
        public int Id { get; set; }
        public string AdressTitle { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }

        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string Zip { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; } = null;




        public UserAdressRES UserAdressDTtoRES()
        {
            UserAdressRES userAdressRES = new UserAdressRES()
            {
                Id = Id,
                Address=Adress,
                AddressTitle=AdressTitle,
                PhoneNumber=PhoneNumber,
                XAxis=XAxis,
                YAxis=YAxis,
                City=City,
                District=District,
                Neighborhood=Neighborhood,
                Zip=Zip
            };

            return userAdressRES;
        }

    }
}
