using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.entity
{
    public class UserSession
    {
        public string UserEmail { get; private set; }
        public string UserRole { get; private set; } 

        private static UserSession _instance;

        private UserSession() { }

        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSession();
                }
                return _instance;
            }
        }

        public void Login(string email, string role) 
        {
            UserEmail = email;
            UserRole = role;
        }

        public void UpdateRole(string role)
        {
            UserRole = role; 
        }

        public void UpdateUserEmail(string email)
        {
            UserEmail = email;
        }

        public void Logout()
        {
            UserEmail = null;
            UserRole = null; 
        }
    }


}
