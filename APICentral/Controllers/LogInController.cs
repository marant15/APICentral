using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace APICentral.Controllers
{
    [Route("crm-api/login")]
    [ApiController]
    public class LogInController : Controller
    {
        // GET: /<controller>/
        public ActionResult<IEnumerable<string>> Index()
        {
            return new string[] { "user", "password" };
        }
    }
}
