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
    public class ProductService:IProductService
    {
        private HttpClient client;
        private String apiUrl;
        private IConfiguration _configuration;
        public ProductService(IConfiguration configuration)
        {
            this._configuration = configuration;
            apiUrl = configuration.GetSection("MicroServices").GetSection("Products").Value;
            client = new HttpClient();
        }
        public async Task<ProductGetDTO> GetProductAsync(String id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/product-management/products/" + id);
                string resp = await ResponseUtility.Verification(response);
                ProductGetDTO productDTO = JsonConvert.DeserializeObject<ProductGetDTO>(resp);
                return productDTO;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Product api not found", 408);
            }
        }

        public async Task<List<ProductGetDTO>> GetAllProductsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/product-management/products");
                string resp = await ResponseUtility.Verification(response);
                List<ProductGetDTO> products = JsonConvert.DeserializeObject<List<ProductGetDTO>>(resp);
                return products;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Product api not found", 408);
            }
        }

        public async Task<List<ProductGetDTO>> GetSomeProductsAsync(string codes)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/product-management/products?ids=" + codes);
                string resp = await ResponseUtility.Verification(response);
                List<ProductGetDTO> products = JsonConvert.DeserializeObject<List<ProductGetDTO>>(resp);
                return products;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Product api not found", 408);
            }
        }

        public async Task<ProductGetDTO> CreateAsync(ProductPostDTO product)
        {
            try {
                String json = JsonConvert.SerializeObject(product, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync(apiUrl + "/product-management/products", data);
                string result = await ResponseUtility.Verification(response);
                return JsonConvert.DeserializeObject<ProductGetDTO>(result);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Product api not found", 408);
            }
        }

        public async Task<ProductGetDTO> UpdateAsync(ProductPutDTO product, String id)
        {
            try {
                String json = JsonConvert.SerializeObject(product, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PutAsync(apiUrl + "/product-management/products/" + id, data);
                string result = await ResponseUtility.Verification(response);
                return JsonConvert.DeserializeObject<ProductGetDTO>(result);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Product api not found", 408);
            }
        }

        public async void DeleteAsync(string id)
        {
            try {
                HttpResponseMessage response = await this.client.DeleteAsync(apiUrl + "/product-management/products/" + id);
                string result = await ResponseUtility.Verification(response);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Product api not found", 408);
            }
        }
    }
}
