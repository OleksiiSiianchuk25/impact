using EfCore.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class AuthServiceImpl
    {
        private readonly ImpactDbContext dbContext;

        public AuthServiceImpl(ImpactDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AuthenticateUser(string email, string password)
        {
            var user = dbContext.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return false; 
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return isPasswordCorrect;
        }
    }
}
