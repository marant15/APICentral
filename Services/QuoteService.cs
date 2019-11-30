using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class QuoteService: IQuoteService
    {
        private HttpClient client;
        private String apiUrl;
        private IConfiguration _configuration;
        public QuoteService(IConfiguration configuration)
        {
            this._configuration = configuration;
            apiUrl = configuration.GetSection("MicroServices").GetSection("Quotes").Value;
            client = new HttpClient();
        }

        public async Task<QuoteSendDTO> GetQuoteAsync(String id)
        {
            var response = await client.GetAsync(apiUrl + "/quoting-management/quotes/" + id);
            var resp = await response.Content.ReadAsStringAsync();
            QuoteDTO quoteDTO = JsonConvert.DeserializeObject<QuoteDTO>(resp);
            return TransformItemListAsync(quoteDTO).Result;
        }

        public async Task<List<QuoteSendDTO>> GetAllQuoteAsync()
        {
            var response = await client.GetAsync(apiUrl + "/quoting-management/quotes");
            var resp = await response.Content.ReadAsStringAsync();
            List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
            List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
            for (int i = 0; i < quotes.Count; i++)
            {
                quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
            }
            return quotesSend;
        }

        public async Task<List<QuoteSendDTO>> GetAllQuoteSoldAsync()
        {
            var response = await client.GetAsync(apiUrl + "/quoting-management/quotes/sold");
            var resp = await response.Content.ReadAsStringAsync();
            List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
            List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
            for (int i = 0; i < quotes.Count; i++)
            {
                quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
            }
            return quotesSend;
        }

        public async Task<List<QuoteSendDTO>> GetAllQuotePendingAsync()
        {
            var response = await client.GetAsync(apiUrl + "/quoting-management/quotes/pending");
            var resp = await response.Content.ReadAsStringAsync();
            List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
            List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
            for (int i = 0; i < quotes.Count; i++)
            {
                quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
            }
            return quotesSend;
        }

        public async Task<QuoteDTO> CreateQuote(QuoteDTO quote)
        {
            try {
                String json = JsonConvert.SerializeObject(quote, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PostAsync(apiUrl + "/quoting-management/quotes", data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<QuoteDTO>(result);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        public async Task<QuoteDTO> UpdateAsync(QuoteDTO quote, String id)
        {
            try
            {
                String json = JsonConvert.SerializeObject(quote, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PutAsync(apiUrl + "/quoting-management/quotes/" + id, data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<QuoteDTO>(result);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async void DeleteAsync(string id)
        {
            try
            {
                var response = await this.client.DeleteAsync(apiUrl + "/quoting-management/quotes/" + id);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private async Task<QuoteSendDTO> TransformItemListAsync(QuoteDTO quoteDTO)
        {
            QuoteSendDTO quoteSendDTO = new QuoteSendDTO();
            quoteSendDTO.Sold = quoteDTO.Sold;
            quoteSendDTO.Date = quoteDTO.Date;
            quoteSendDTO.QuoteName = quoteDTO.QuoteName;
            var clientResponse = await client.GetAsync(_configuration.GetSection("MicroServices").GetSection("Clients").Value + "/client-management/clients/" + quoteDTO.ClientCode);
            ClientDTO clientDTO = JsonConvert.DeserializeObject<ClientDTO>(clientResponse.Content.ReadAsStringAsync().Result);
            quoteSendDTO.Client = clientDTO;
            quoteSendDTO.QuoteListItems = new List<ListItemSendDTO>();
            if (quoteDTO.QuoteListItems.Count > 0)
            {
                string codes = quoteDTO.QuoteListItems[0].ProductCode;
                for (int i = 1; i < quoteDTO.QuoteListItems.Count; i++)
                {
                    codes = codes +","+ quoteDTO.QuoteListItems[i].ProductCode;
                }
                var productResponse = await client.GetAsync(_configuration.GetSection("MicroServices").GetSection("Products").Value + "/product-management/products?ids=" + codes);
                List<ProductGetDTO> productos = JsonConvert.DeserializeObject<List<ProductGetDTO>>(productResponse.Content.ReadAsStringAsync().Result);
                for (int i = 0; i < quoteDTO.QuoteListItems.Count; i++)
                {
                    ListItemSendDTO item= new ListItemSendDTO();
                    item.Price = quoteDTO.QuoteListItems[i].Price;
                    item.Quantity = quoteDTO.QuoteListItems[i].Quantity;
                    item.QuoteName = quoteDTO.QuoteListItems[i].QuoteName;
                    item.Product = productos[i];
                    quoteSendDTO.QuoteListItems.Add(item);
                }
            }
            return quoteSendDTO;
        }
    }
}
