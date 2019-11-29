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

        Task<bool> CreateAsync(ClientDTO client);

        Task<bool> UpdateAsync(ClientUpdateDTO client, String id);

        Task<bool> DeleteAsync(string id);
    }
}
