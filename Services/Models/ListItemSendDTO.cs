using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ListItemSendDTO
    {
        public string QuoteName { get; set; }

        public ProductGetDTO Product { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
