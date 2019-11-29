using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ProductGetDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Stock { get; set; }
    }
}
