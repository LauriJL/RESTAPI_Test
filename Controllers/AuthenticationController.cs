using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restful_Lopputehtava_LauriLeskinen.Services;

namespace Restful_Lopputehtava_LauriLeskinen.Controllers
{
    [Route("northwind/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;
        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Models.Logins model)
        {
            var user = _authenticateService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Käyttäjätunnus tai salasana on virheellinen" });

            return Ok(user); //Return to frontend
        }
    }
}
