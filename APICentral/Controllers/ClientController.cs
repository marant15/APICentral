using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;
using Services.Models;

namespace APICentral.Controllers
{
    [Route("crm-api/clients")]
    public class ClientController : Controller
    {
        private IClientService _clientService;
        private IConfiguration _configuration;

        public ClientController(IConfiguration configuration, IClientService clientService)
        {
            _configuration = configuration;
            _clientService = clientService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_clientService.GetAllClientAsync().Result);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(String id)
        {
            return Ok(_clientService.GetClientAsync(id).Result);
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]ClientDTO client)
        {
            return Ok(_clientService.CreateAsync(client).Result);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(String id, [FromBody]ClientUpdateDTO client)
        {
            return Ok(_clientService.UpdateAsync(client,id).Result);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(String id)
        {
            _clientService.DeleteAsync(id);
            return Ok();
        }
    }
}
