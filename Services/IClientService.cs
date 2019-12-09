using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IClientService
    {
        Task<ClientDTO> GetClientAsync(String id);

        Task<List<ClientDTO>> GetAllClientAsync();

        Task<ClientDTO> CreateAsync(ClientDTO client);

        Task<ClientDTO> UpdateAsync(ClientUpdateDTO client, String id);

        void DeleteAsync(string id);
    }
}
