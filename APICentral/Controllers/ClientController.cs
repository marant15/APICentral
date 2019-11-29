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
        private ClientService _clientService;
        private IConfiguration _configuration;

        public ClientController(IConfiguration configuration, IClientService clientService)
        {
            _configuration = configuration;
            _clientService = new ClientService(configuration);
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
            bool status = _clientService.CreateAsync(client).Result;
            if (status)
            {
                return Ok();
            }
            else
            {
                return BadRequest("error");
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(String id, [FromBody]ClientUpdateDTO client)
        {
            bool status = _clientService.UpdateAsync(client,id).Result;
            if (status)
            {
                return Ok();
            }
            else
            {
                return BadRequest("error");
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(String id)
        {
            bool status = _clientService.DeleteAsync(id).Result;
            if (status)
            {
                return Ok();
            }
            else
            {
                return BadRequest("error");
            }
        }
    }
}
