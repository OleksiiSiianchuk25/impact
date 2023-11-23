using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EfCore.service.impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly ImpactDbContext context;
        private readonly RoleServiceImpl roleService;

        public UserServiceImpl(ImpactDbContext context, RoleServiceImpl roleService)
        {
            this.context = context;
            this.roleService = roleService;
        }

        public List<User> GetAllUsers()
        {
            return context.Users.ToList();
        }

        public User GetUserByEmail(string userEmail)
        {
            User user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                throw new ApplicationException("User with email: " + userEmail + " does not exist!");
            }
            return user;
        }

        public User GetUserById(int userId)
        {
            User user = context.Users.Find(userId);
            if (user == null)
            {
                throw new ApplicationException("User with id: " + userId + " does not exist!");
            }
            return user;
        }
        public void DeleteUserById(int userId)
        {
            User user = GetUserById(userId);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public void RegisterUser(UserDTO userDTO)
        {

            if (userDTO.Password != userDTO.ConfirmPassword)
            {
                throw new InvalidOperationException("Пароль та його підтвердження не співпадають.");
            }

            Role userRole = context.Roles.Find(userDTO.RoleId);

            User newUser = new User
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                MiddleName = userDTO.MiddleName,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                RoleRef = userDTO.RoleId,
                RoleRefNavigation = userRole
            };

            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public List<User> GetOrderers()
        {
            return context.Users
                .Where(u => u.RoleRef == roleService.GetOrdererRole().RoleId).ToList();
        }

        public List<User> GetVolunteers()
        {
            return context.Users
                .Where(u => u.RoleRef == roleService.GetVolunteerRole().RoleId).ToList();
        }
    }
}
