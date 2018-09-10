using KahveAkademisi.Entities.ResponseModels;
using KahveAkademisi.Entities.ResponseModels.Client;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.DbModels
{
    public class AppUser:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Language Language { get; set; }
        public DateTime CreateDate { get; set; }
        public string VerifyCode { get; set; }
        public string Tckn { get; set; }

        public List<UserAddress> UserAdresses { get; set; }
        public List<Order> Orders { get; set; }

        public AppUserRES AppUserDTtoRES()
        {
            AppUserRES appUserRES = new AppUserRES()
            {
                FirstName = FirstName,
                LastName = LastName,
                UserName = UserName
            };

            return appUserRES;
        }

    }
}
