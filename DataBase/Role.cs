using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    class Role
    {
        public int RoleId { get; set; }

        public string Name { get; set; }

        public ICollection<User> UserRoles { get; set; }
    }
}
