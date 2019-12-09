using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ResponseUtility
    {
        public static Task<string> Verification(HttpResponseMessage response)
        {
            int statusCode = (int)response.StatusCode;
            if (statusCode == 200)
            {
                return response.Content.ReadAsStringAsync();
            }
            else
            {
                string msg = response.Content.ReadAsStringAsync().Result;
                throw new ServicesException(msg, statusCode);
            }
        }
    }
}
