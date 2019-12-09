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
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/quote-management/quotes/" + id);
                string resp = await ResponseUtility.Verification(response);
                QuoteDTO quoteDTO = JsonConvert.DeserializeObject<QuoteDTO>(resp);
                return TransformItemListAsync(quoteDTO).Result;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        public async Task<List<QuoteSendDTO>> GetAllQuoteAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/quote-management/quotes");
                string resp = await ResponseUtility.Verification(response);
                List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
                List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
                for (int i = 0; i < quotes.Count; i++)
                {
                    quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
                }
                return quotesSend;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        public async Task<List<QuoteSendDTO>> GetAllQuoteSoldAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/quote-management/quotes/sold");
                string resp = await ResponseUtility.Verification(response);
                List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
                List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
                for (int i = 0; i < quotes.Count; i++)
                {
                    quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
                }
                return quotesSend;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        public async Task<List<QuoteSendDTO>> GetAllQuotePendingAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/quote-management/quotes/pending");
                string resp = await ResponseUtility.Verification(response);
                List<QuoteDTO> quotes = JsonConvert.DeserializeObject<List<QuoteDTO>>(resp);
                List<QuoteSendDTO> quotesSend = new List<QuoteSendDTO>();
                for (int i = 0; i < quotes.Count; i++)
                {
                    quotesSend.Add(TransformItemListAsync(quotes[i]).Result);
                }
                return quotesSend;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        public async Task<QuoteDTO> CreateQuote(QuoteDTO quote)
        {
            try {
                String json = JsonConvert.SerializeObject(quote, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync(apiUrl + "/quote-management/quotes", data);
                string result = await ResponseUtility.Verification(response);
                return JsonConvert.DeserializeObject<QuoteDTO>(result);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        public async Task<QuoteDTO> UpdateAsync(QuoteDTO quote, String id)
        {
            try
            {
                String json = JsonConvert.SerializeObject(quote, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PutAsync(apiUrl + "/quote-management/quotes/" + id, data);
                string result = await ResponseUtility.Verification(response);
                return JsonConvert.DeserializeObject<QuoteDTO>(result);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        public async void DeleteAsync(string id)
        {
            try
            {
                HttpResponseMessage response = await this.client.DeleteAsync(apiUrl + "/quote-management/quotes/" + id);
                string result = await ResponseUtility.Verification(response);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Quote api not found", 408);
            }
        }

        private async Task<QuoteSendDTO> TransformItemListAsync(QuoteDTO quoteDTO)
        {
            QuoteSendDTO quoteSendDTO = new QuoteSendDTO();
            quoteSendDTO.Sold = quoteDTO.Sold;
            quoteSendDTO.Date = quoteDTO.Date;
            quoteSendDTO.QuoteName = quoteDTO.QuoteName;
            HttpResponseMessage clientResponse = await client.GetAsync(_configuration.GetSection("MicroServices").GetSection("Clients").Value + "/client-management/clients/" + quoteDTO.ClientCode);
            string clientResult = await ResponseUtility.Verification(clientResponse);
            ClientDTO clientDTO = JsonConvert.DeserializeObject<ClientDTO>(clientResult);
            quoteSendDTO.Client = clientDTO;
            quoteSendDTO.QuoteListItems = new List<ListItemSendDTO>();
            if (quoteDTO.QuoteLineItems.Count > 0)
            {
                string codes = quoteDTO.QuoteLineItems[0].ProductCode;
                for (int i = 1; i < quoteDTO.QuoteLineItems.Count; i++)
                {
                    codes = codes +","+ quoteDTO.QuoteLineItems[i].ProductCode;
                }
                HttpResponseMessage productResponse = await client.GetAsync(_configuration.GetSection("MicroServices").GetSection("Products").Value + "/product-management/products?ids=" + codes);
                string productResult = await ResponseUtility.Verification(productResponse);
                List<ProductGetDTO> productos = JsonConvert.DeserializeObject<List<ProductGetDTO>>(productResult);
                for (int i = 0; i < quoteDTO.QuoteLineItems.Count; i++)
                {
                    ListItemSendDTO item= new ListItemSendDTO();
                    item.Price = quoteDTO.QuoteLineItems[i].Price;
                    item.Quantity = quoteDTO.QuoteLineItems[i].Quantity;
                    item.QuoteName = quoteDTO.QuoteLineItems[i].QuoteName;
                    item.Product = productos.Find(x => x.Code.Equals(quoteDTO.QuoteLineItems[i].ProductCode));
                    quoteSendDTO.QuoteListItems.Add(item);
                }
            }
            return quoteSendDTO;
        }
    }
}
