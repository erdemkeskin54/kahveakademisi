using KahveAkademisi.WebAPI.Models.Abstract;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KahveAkademisi.WebAPI
{
    public class MyHub : Hub<ITypedHubClient>
    {
        public void deneme()
        {
            Clients.All.broadCaseMessage("merhaba");
        }
    }
}
