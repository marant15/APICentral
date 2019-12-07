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
            try
            {
                var response = await client.GetAsync(apiUrl + "/quote-management/quotes/" + id);
                var resp = await response.Content.ReadAsStringAsync();
                QuoteDTO quoteDTO = JsonConvert.DeserializeObject<QuoteDTO>(resp);
                return TransformItemListAsync(quoteDTO).Result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<QuoteSendDTO>> GetAllQuoteAsync()
        {
            try
            {
                var response = await client.GetAsync(apiUrl + "/quote-management/quotes");
                var resp = await response.Content.ReadAsStringAsync();
                List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
                List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
                for (int i = 0; i < quotes.Count; i++)
                {
                    quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
                }
                return quotesSend;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<QuoteSendDTO>> GetAllQuoteSoldAsync()
        {
            try
            {
                var response = await client.GetAsync(apiUrl + "/quote-management/quotes/sold");
                var resp = await response.Content.ReadAsStringAsync();
                List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
                List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
                for (int i = 0; i < quotes.Count; i++)
                {
                    quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
                }
                return quotesSend;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<QuoteSendDTO>> GetAllQuotePendingAsync()
        {
            try
            {
                var response = await client.GetAsync(apiUrl + "/quote-management/quotes/pending");
                var resp = await response.Content.ReadAsStringAsync();
                List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
                List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
                for (int i = 0; i < quotes.Count; i++)
                {
                    quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
                }
                return quotesSend;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<QuoteDTO> CreateQuote(QuoteDTO quote)
        {
            try {
                String json = JsonConvert.SerializeObject(quote, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PostAsync(apiUrl + "/quote-management/quotes", data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<QuoteDTO>(result);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<QuoteDTO> UpdateAsync(QuoteDTO quote, String id)
        {
            try
            {
                String json = JsonConvert.SerializeObject(quote, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PutAsync(apiUrl + "/quote-management/quotes/" + id, data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<QuoteDTO>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async void DeleteAsync(string id)
        {
            try
            {
                var response = await this.client.DeleteAsync(apiUrl + "/quote-management/quotes/" + id);
            }
            catch (Exception e)
            {
                throw e;
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
            if (quoteDTO.QuoteLineItems.Count > 0)
            {
                string codes = quoteDTO.QuoteLineItems[0].ProductCode;
                for (int i = 1; i < quoteDTO.QuoteLineItems.Count; i++)
                {
                    codes = codes +","+ quoteDTO.QuoteLineItems[i].ProductCode;
                }
                var productResponse = await client.GetAsync(_configuration.GetSection("MicroServices").GetSection("Products").Value + "/product-management/products?ids=" + codes);
                List<ProductGetDTO> productos = JsonConvert.DeserializeObject<List<ProductGetDTO>>(productResponse.Content.ReadAsStringAsync().Result);
                for (int i = 0; i < quoteDTO.QuoteLineItems.Count; i++)
                {
                    ListItemSendDTO item= new ListItemSendDTO();
                    item.Price = quoteDTO.QuoteLineItems[i].Price;
                    item.Quantity = quoteDTO.QuoteLineItems[i].Quantity;
                    item.QuoteName = quoteDTO.QuoteLineItems[i].QuoteName;
                    item.Product = productos[i];
                    quoteSendDTO.QuoteListItems.Add(item);
                }
            }
            return quoteSendDTO;
        }
    }
}
