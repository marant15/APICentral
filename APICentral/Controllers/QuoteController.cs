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
    [Route("crm-api/quotes")]
    public class QuoteController : Controller
    {
        private IQuoteService _quoteService;
        private IConfiguration _configuration;

        public QuoteController(IConfiguration configuration, IQuoteService quoteService)
        {
            _configuration = configuration;
            _quoteService = quoteService;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_quoteService.GetAllQuoteAsync().Result);
        }

        [HttpGet]
        [Route("pending")]
        public IActionResult GetAllpending()
        {
            return Ok(_quoteService.GetAllQuotePendingAsync().Result);
        }

        [HttpGet]
        [Route("sold")]
        public IActionResult GetAllsold()
        {
            return Ok(_quoteService.GetAllQuoteSoldAsync().Result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(String id)
        {
            return Ok(_quoteService.GetQuoteAsync(id).Result);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody]QuoteDTO quote)
        {
            return Ok(_quoteService.CreateQuote(quote).Result);
        }
        
        [HttpPut("{id}")]
        public IActionResult Put(String id, [FromBody]QuoteDTO quote)
        {
            return Ok(_quoteService.UpdateAsync(quote, id).Result);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(String id)
        {
            _quoteService.DeleteAsync(id);
            return Ok();
        }
    }
}
