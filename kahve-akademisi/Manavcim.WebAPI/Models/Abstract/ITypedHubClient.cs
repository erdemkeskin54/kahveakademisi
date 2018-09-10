using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KahveAkademisi.WebAPI.Models.Abstract
{
    public interface ITypedHubClient
    {
        Task deneme2();
        Task broadCaseMessage(string message);
        Task checkoutNav();
    }
}
