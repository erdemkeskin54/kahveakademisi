using System;
using System.Collections.Generic;
using System.Text;

namespace KahveAkademisi.Entities.Infrastructure
{
    public class ApiResponsive
    {
        public bool IsSucces { get; set; }
        public object ReturnObject { get; set; }
        public List<ErrorModel> ErrorContent { get; set; }

        public ApiResponsive()
        {
            ErrorContent = new List<ErrorModel>();
        }
    }
}
