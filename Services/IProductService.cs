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

        Task<ProductGetDTO> CreateAsync(ProductPostDTO product);

        Task<ProductGetDTO> UpdateAsync(ProductPutDTO product, String id);

        void DeleteAsync(string id);
    }
}
