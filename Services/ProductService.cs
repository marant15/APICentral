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
                var response = await client.GetAsync(apiUrl + "/product-management/products/" + id);
                var resp = await response.Content.ReadAsStringAsync();
                ProductGetDTO productDTO = JsonConvert.DeserializeObject<ProductGetDTO>(resp);
                return productDTO;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<ProductGetDTO>> GetAllProductsAsync()
        {
            try
            {
                var response = await client.GetAsync(apiUrl + "/product-management/products");
                var resp = await response.Content.ReadAsStringAsync();
                List<ProductGetDTO> products = JsonConvert.DeserializeObject<List<ProductGetDTO>>(resp);
                return products;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<ProductGetDTO>> GetSomeProductsAsync(string codes)
        {
            try
            {
                var response = await client.GetAsync(apiUrl + "/product-management/products?ids=" + codes);
                var resp = await response.Content.ReadAsStringAsync();
                List<ProductGetDTO> products = JsonConvert.DeserializeObject<List<ProductGetDTO>>(resp);
                return products;
            }catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<ProductGetDTO> CreateAsync(ProductPostDTO product)
        {
            try {
                String json = JsonConvert.SerializeObject(product, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PostAsync(apiUrl + "/product-management/products", data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ProductGetDTO>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
}

        public async Task<ProductGetDTO> UpdateAsync(ProductPutDTO product, String id)
        {
            try {
                String json = JsonConvert.SerializeObject(product, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PutAsync(apiUrl + "/product-management/products/" + id, data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ProductGetDTO>(result);
            }
            catch (Exception e)
            {
                throw e;
            }
}

        public async void DeleteAsync(string id)
        {
            try {
                var response = await this.client.DeleteAsync(apiUrl + "/product-management/products/" + id);
            }
            catch (Exception e)
            {
                throw e;
            }
}
    }
}
