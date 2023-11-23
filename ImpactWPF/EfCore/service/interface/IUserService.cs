using EfCore.dto;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service
{
    internal interface IUserService
    {
        List<User> GetAllUsers();

        User GetUserById(int userId);

        User GetUserByEmail(string userEmail);

        void RegisterUser(UserDTO newUser);

        void DeleteUserById(int userId);

        public List<User> GetOrderers();

        public List<User> GetVolunteers();
    }
}
