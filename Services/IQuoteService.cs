using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IQuoteService
    {
        Task<QuoteSendDTO> GetQuoteAsync(String id);

        Task<List<QuoteSendDTO>> GetAllQuoteAsync();

        Task<List<QuoteSendDTO>> GetAllQuoteSoldAsync();

        Task<List<QuoteSendDTO>> GetAllQuotePendingAsync();

        Task<QuoteDTO> CreateQuote(QuoteDTO quote);

        Task<QuoteDTO> UpdateAsync(QuoteDTO quote, String id);

        void DeleteAsync(string id);
    }
}
