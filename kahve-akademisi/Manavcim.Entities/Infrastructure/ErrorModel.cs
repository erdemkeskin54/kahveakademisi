using System;
using System.Collections.Generic;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.Infrastructure
{
    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage {get;set;}
    }
}
