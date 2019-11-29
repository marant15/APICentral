using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetProductAsync(String id);

        Task<List<ProductGetDTO>> GetAllProductsAsync();

        Task<List<ProductGetDTO>> GetSomeProductsAsync(string codes);

        Task<bool> CreateAsync(ProductPostDTO product);

        Task<bool> UpdateAsync(ProductPutDTO product, String id);

        Task<bool> DeleteAsync(string id);
    }
}
