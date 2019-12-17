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
        public List<sessionDTO> sessions  = new List<sessionDTO>();

        public AuthorizationService ()
        {
            
        }

        public Guid login(string authorization)
        {
            Console.WriteLine(authorization);
            string token = authorization;
            string userName = token.Split(":")[0];
            string codepassWord = token.Split(":")[1];
            string passWord = Encoding.UTF8.GetString(Convert.FromBase64String(codepassWord), 0, Convert.FromBase64String(codepassWord).Length);
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
                    sessionID = Guid.NewGuid();
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
            string token = bearer.Split(" ")[1];
            bool isSessionValid = sessions.Exists(session => session.session.ToString().Equals(token));
            return isSessionValid;
        }

        private List<UserDTO> getAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>() {
                new UserDTO() { Username = "admin", PassWord = "admin" },
                new UserDTO() { Username = "user", PassWord = "user" }
            };
            return users;
        }
    }
}
