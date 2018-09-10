using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static KahveAkademisi.Entities.Infrastructure.Enums;

namespace KahveAkademisi.Entities.RequestModels.AuthControllerModels
{
    public class CredentialModel
    {
        [Required(ErrorMessage ="TelefonNumarasiZorunlu")]
        [MinLength(11,ErrorMessage = "TelefonNumarasiCokKisa"),MaxLength(11,ErrorMessage ="TelefonNumarasiCokUzun")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="SifreZorunlu")]
        [MinLength(6,ErrorMessage ="SifreCokKisa"),MaxLength(10,ErrorMessage ="SifreCokUzun")]
        public string Password { get; set; }

    }
}
