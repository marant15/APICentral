using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAuthorizationService
    {
        Guid login(string authorization);
        Boolean ValidateUser(string bearer);
    }
}
