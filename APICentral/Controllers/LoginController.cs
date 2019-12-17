using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
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
        [HttpPost]

        public IActionResult Login([FromHeader]string authorization)
        {
            Guid sessionId = _authService.login(authorization);
            if (sessionId != Guid.Empty)
            {
                HttpContext.Response.Headers.Add("Authorization", "Bearer " + sessionId.ToString());
                Console.WriteLine("FOTO DE SEMESTRE: " + HttpContext.Response.Headers["Authorization"]);
                return Ok();
            }
            else 
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            
        }
    }
}
