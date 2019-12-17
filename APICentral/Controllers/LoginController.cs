using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APICentral.Controllers
{
    public class LoginController : Controller
    {
        private IAuthorizationService _authService;
        public LoginController(IAuthorizationService auth) 
        {
           _authService = auth;
        }

        [Route("crm-api/login")]
        public IActionResult Login([FromHeader]string authorization)
        {
            Guid sessionId = _authService.login(authorization);
            if (sessionId != Guid.Empty)
            {
                HttpContext.Response.Headers.Add("Authorization", sessionId.ToString());
                return Ok();
            }
            else 
            {
                return Unauthorized();
            }
            
        }
        [Route("crm-api/validate")]
        public IActionResult ValidateSession([FromHeader]string authorization) 
        {
            if (_authService.ValidateUser(authorization))
                return Ok();
            else
                return Unauthorized();
        }
    }
}
