using Restful_Lopputehtava_LauriLeskinen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restful_Lopputehtava_LauriLeskinen.Services
{
    public interface IAuthenticateService
    {
        Logins Authenticate(string Username, string Password);
    }
}
