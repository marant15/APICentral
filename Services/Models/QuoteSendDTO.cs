using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class QuoteSendDTO
    {
        public string QuoteName { get; set; }

        public ClientDTO Client { get; set; }

        public string Date { get; set; }

        public bool Sold { get; set; }

        public List<ListItemSendDTO> QuoteListItems { get; set; }
    }
}
