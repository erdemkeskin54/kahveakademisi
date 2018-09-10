using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.Entities.ResponseModels.Client
{
    public class UserAdressRES
    {
        public int Id { get; set; }
        public string AddressTitle { get; set; }
        public string XAxis { get; set; }
        public string YAxis { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string Zip { get; set; }

    }
}
