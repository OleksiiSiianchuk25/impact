using EfCore.context;
using EfCore.entity;
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
        private readonly UserServiceImpl userService;

        public AuthServiceImpl(ImpactDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.userService = new UserServiceImpl(dbContext);
        }

        public bool AuthenticateUser(string email, string password)
        {
            var user = userService.GetUserByEmail(email);

            if (user == null)
            {
                return false; 
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.Password);

            return isPasswordCorrect;
        }
        public Role GetUserRoleByEmail(string userEmail)
        {
            return userService.GetUserRoleByEmail(userEmail);
        }
    }
}
