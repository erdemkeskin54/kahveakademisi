using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.RequestModels.AuthControllerModels
{
    public class RegisterModel_Req
    {
        [Required(ErrorMessage = "TelefonNumarasiZorunlu")]
        [MinLength(11, ErrorMessage = "TelefonNumarasiCokKisa"), MaxLength(11, ErrorMessage = "TelefonNumarasiCokUzun")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "SifreZorunlu")]
        [MinLength(4, ErrorMessage = "SifreCokKisa"), MaxLength(10, ErrorMessage = "SifreCokUzun")]
        public string Password { get; set; }

        [Required(ErrorMessage ="DogrulamaKoduZorunlu")]
        [MinLength(4, ErrorMessage="DogrulamaKoduKisa"),MaxLength(4 ,ErrorMessage ="DogrulamaKoduUzun")]
        public string VerifyCode { get; set; }

        [Required(ErrorMessage = "AdZorunlu")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "SoyAdZorunlu")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="DilZorunlu")]
        public Language Language { get; set; }
    }
}
