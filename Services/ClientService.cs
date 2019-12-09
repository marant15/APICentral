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
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/client-management/clients/" + id);
                string resp = await ResponseUtility.Verification(response);
                ClientDTO clientDTO = JsonConvert.DeserializeObject<ClientDTO>(resp);
                return clientDTO;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Client api not found", 408);
            }
        }

        public async Task<List<ClientDTO>> GetAllClientAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl + "/client-management/clients");
                string resp = await ResponseUtility.Verification(response);
                List<ClientDTO> clients = JsonConvert.DeserializeObject<List<ClientDTO>>(resp);
                return clients;
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Client api not found", 408);
            }
        }

        public async Task<ClientDTO> CreateAsync(ClientDTO client)
        {
            try {
                String json = JsonConvert.SerializeObject(client,new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync(apiUrl+ "/client-management/clients", data);
                string result = await ResponseUtility.Verification(response);
                return JsonConvert.DeserializeObject<ClientDTO>(result);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Client api not found", 408);
            }
        }

        public async Task<ClientDTO> UpdateAsync(ClientUpdateDTO client, String id)
        {
            try { 
                String json = JsonConvert.SerializeObject(client, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PutAsync(apiUrl + "/client-management/clients/"+id, data);
                string result = await ResponseUtility.Verification(response);
                return JsonConvert.DeserializeObject<ClientDTO>(result);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Client api not found", 408);
            }
        }

        public async void DeleteAsync(string id)
        {
            try
            {
                HttpResponseMessage response = await this.client.DeleteAsync(apiUrl + "/client-management/clients/" + id);
                string result = await ResponseUtility.Verification(response);
            }
            catch (ServicesException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw new ServicesException("Client api not found", 408);
            }
        }
    }
}
