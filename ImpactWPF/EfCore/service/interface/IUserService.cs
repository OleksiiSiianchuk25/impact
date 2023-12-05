using EfCore.dto;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.service
{
    public interface IUserService
    {
        List<User> GetAllUsers();

        User GetUserById(int userId);

        User GetUserByEmail(string userEmail);

        void RegisterUser(UserDTO newUser);

        void DeleteUserById(int userId);

        List<User> GetOrderers();

        List<User> GetVolunteers();

        void ChangePassword(string email, string newPassword);

        void UpdateUserPassword(User currentUser, string userPassword);

        public void UpdateUserData(User currentUser, string userEmail, string userLastName, string userFirstName, string userMiddleName, string userPhoneNumber, string userRole);
    }
}
