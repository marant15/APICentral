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
            var response = await client.GetAsync(apiUrl + "/product-management/products/" + id);
            var resp = await response.Content.ReadAsStringAsync();
            ProductGetDTO productDTO = JsonConvert.DeserializeObject<ProductGetDTO>(resp);
            return productDTO;
        }

        public async Task<List<ProductGetDTO>> GetAllProductsAsync()
        {
            var response = await client.GetAsync(apiUrl + "/product-management/products");
            var resp = await response.Content.ReadAsStringAsync();
            List<ProductGetDTO> products = JsonConvert.DeserializeObject<List<ProductGetDTO>>(resp);
            return products;
        }

        public async Task<List<ProductGetDTO>> GetSomeProductsAsync(string codes)
        {
            var response = await client.GetAsync(apiUrl + "/product-management/products?ids="+codes);
            var resp = await response.Content.ReadAsStringAsync();
            List<ProductGetDTO> products = JsonConvert.DeserializeObject<List<ProductGetDTO>>(resp);
            return products;
        }

        public async Task<bool> CreateAsync(ProductPostDTO product)
        {
            String json = JsonConvert.SerializeObject(product, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(apiUrl + "/product-management/products", data);
            if ((int)response.StatusCode == 200)
                return true;
            else return false;
        }

        public async Task<bool> UpdateAsync(ProductPutDTO product, String id)
        {
            String json = JsonConvert.SerializeObject(product, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.client.PutAsync(apiUrl + "/product-management/products/" + id, data);
            if ((int)response.StatusCode == 200)
                return true;
            else return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await this.client.DeleteAsync(apiUrl + "/product-management/products/" + id);
            if ((int)response.StatusCode == 200)
                return true;
            else return false;
        }
    }
}
