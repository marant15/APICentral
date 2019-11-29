using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string FirstSurname { get; set; }

        public string SecondSurname { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
