using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ClientDTO
    {
        public string ClientId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string CI { get; set; }

        public string Address { get; set; }

        public int Ranking { get; set; }

        public long Phone { get; set; }
    }
}
