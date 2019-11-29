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

        public async Task<bool> CreateAsync(ClientDTO client)
        {
            String json = JsonConvert.SerializeObject(client,new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.client.PostAsync(apiUrl+ "/client-management/clients", data);
            if ((int)response.StatusCode == 200)
                return true;
            else return false;
        }

        public async Task<bool> UpdateAsync(ClientUpdateDTO client, String id)
        {
            String json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.client.PutAsync(apiUrl + "/client-management/clients/"+id, data);
            if ((int)response.StatusCode == 200)
                return true;
            else return false;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var response = await this.client.DeleteAsync(apiUrl + "/client-management/clients/" + id);
            if ((int)response.StatusCode == 200)
                return true;
            else return false;
        }
    }
}
