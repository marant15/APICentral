using Microsoft.Extensions.Configuration;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private HttpClient client;
        //private String apiUrl;
        private IConfiguration _configuration;

        public List<sessionDTO> sessions  = new List<sessionDTO>();

        public AuthorizationService (IConfiguration configuration)
        {
            this._configuration = configuration;
            //apiUrl = configuration.GetSection("MicroServices").GetSection("Quotes").Value;
            client = new HttpClient();
        }

        public Guid login(string authorization)
        {
            string token = authorization.Split(" ")[1];
            string decodeToken = Encoding.UTF8.GetString(Convert.FromBase64String(token), 0, Convert.FromBase64String(token).Length);
            string userName = decodeToken.Split(":")[0];
            string passWord = decodeToken.Split(":")[1];
            Guid session = GetSessionGuid(userName, passWord);
            return session;
        }

        private Guid GetSessionGuid(string userName, string passWord)
        {
            List<UserDTO> users = this.getAllUsers();
            Boolean valido = false;
            Guid sessionID = Guid.Empty;
            for (int i = 0; i < users.Count; i++) 
            {
                if (users[i].Username == userName && users[i].PassWord == passWord)
                {
                    valido = true;
                    sessionID = new Guid();
                }
            }

            if (valido)
            {
                sessionDTO session = new sessionDTO();
                session.userName = userName;
                session.session = sessionID;
                this.sessions.Add(session);

            }
            return sessionID;
        }

        public Boolean ValidateUser(string bearer) 
        {
            return sessions.Exists(session => session.session.Equals(bearer.Replace("bearer", "")));
        }

        private List<UserDTO> getAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>() {
                new UserDTO() { Username = "nachoPicante", PassWord = "nachitorico" },
                new UserDTO() { Username = "WowBlast", PassWord = "jhoncito" }
            };
            return users;
        }
    }
}
