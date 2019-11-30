using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class QuoteDTO
    {
        public string QuoteName { get; set; }

   		public string ClientCode { get; set; }

        public string Date { get; set; }

        public bool Sold { get; set; }

        public List<ListItemDTO> QuoteListItems { get; set; } 

    }
}
