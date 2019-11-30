using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Services.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;

namespace Services
{
    public class ClientService : IClientService
    {
        private HttpClient client;
        private String apiUrl;
        private IConfiguration _configuration;
        public ClientService(IConfiguration configuration)
        {
            this._configuration = configuration;
            apiUrl = configuration.GetSection("MicroServices").GetSection("Clients").Value;
            client = new HttpClient();
        }

        public async Task<ClientDTO> GetClientAsync(String id)
        {
            var response = await client.GetAsync(apiUrl+"/client-management/clients/"+id);
            var resp = await response.Content.ReadAsStringAsync();
            ClientDTO clientDTO = JsonConvert.DeserializeObject<ClientDTO>(resp);
            return clientDTO;
        }

        public async Task<List<ClientDTO>> GetAllClientAsync()
        {
            var response = await client.GetAsync(apiUrl + "/client-management/clients");
            var resp = await response.Content.ReadAsStringAsync();
            List<ClientDTO> clients = JsonConvert.DeserializeObject<List<ClientDTO>>(resp);
            return clients;
        }

        public async Task<ClientDTO> CreateAsync(ClientDTO client)
        {
            try {
                String json = JsonConvert.SerializeObject(client,new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PostAsync(apiUrl+ "/client-management/clients", data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ClientDTO>(result);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<ClientDTO> UpdateAsync(ClientUpdateDTO client, String id)
        {
            try { 
                String json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.client.PutAsync(apiUrl + "/client-management/clients/"+id, data);
                string result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ClientDTO>(result);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        public async void DeleteAsync(string id)
        {
            try
            {
                var response = await this.client.DeleteAsync(apiUrl + "/client-management/clients/" + id);
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
