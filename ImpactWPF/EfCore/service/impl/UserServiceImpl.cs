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
        private readonly VerificationCodeManager verificationCodeManager;

        public UserServiceImpl(ImpactDbContext context)
        {
            this.context = context;
            this.roleService = new RoleServiceImpl(context);
        }

        public List<User> GetAllUsers()
        {
            return context.Users.ToList();
        }

        public User GetUserByEmail(string userEmail)
        {
            User user = context.Users.FirstOrDefault(u => u.Email == userEmail);
            return user;
        }

        public Role GetUserRoleByEmail(string userEmail)
        {
            User user = GetUserByEmail(userEmail);
            if (user == null)
            {
                throw new ApplicationException("Користувач з електронною поштою: " + userEmail + " не існує!");
            }
            return context.Roles.Find(user.RoleRef);
        }

        public User GetUserById(int userId)
        {
            User user = context.Users.Find(userId);
            if (user == null)
            {
                throw new ApplicationException("Користувач з id: " + userId + " не існує!");
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

        public void ChangePassword(string email, string newPassword)
        {
            User user = GetUserByEmail(email);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
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

        public void UpdateUserData(User currentUser, string userEmail, string userLastName, string userFirstName, string userMiddleName, string userPhoneNumber, string userRole)
        {
            try
            {

                if (userRole == "Волонтер")
                {
                    userRole = "ROLE_VOLUNTEER";
                }
                else if (userRole == "Адмін")
                {
                    userRole = "ROLE_ADMIN";
                }
                else
                {
                    userRole = "ROLE_ORDERER";
                }

                Role newRole = context.Roles.FirstOrDefault(r => r.RoleName == userRole);
                UserSession.Instance.UpdateRole(userRole);
                UserSession.Instance.UpdateUserEmail(userEmail);

                currentUser.Email = userEmail;
                currentUser.LastName = userLastName;
                currentUser.FirstName = userFirstName;
                currentUser.MiddleName = userMiddleName;
                currentUser.PhoneNumber = userPhoneNumber;

                currentUser.RoleRef = newRole.RoleId;
                currentUser.RoleRefNavigation = newRole;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Помилка при оновленні даних користувача: {ex.Message}");
            }
        }

        public void UpdateUserPassword(User currentUser, string userPassword)
        {
            try
            {
                currentUser.Password = BCrypt.Net.BCrypt.HashPassword(userPassword);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Помилка при оновленні паролю користувача: {ex.Message}");
            }
        }

        public void AdminUpdateUserData(User currentUser, string userEmail, string userLastName, string userFirstName, string userMiddleName, string userPhoneNumber, string? userRole)
        {
             try
            {

                if (userRole == "Волонтер")
                {
                    userRole = "ROLE_VOLUNTEER";
                }
                else if (userRole == "Адмін")
                {
                    userRole = "ROLE_ADMIN";
                }
                else
                {
                    userRole = "ROLE_ORDERER";
                }

                Role newRole = context.Roles.FirstOrDefault(r => r.RoleName == userRole);

                currentUser.Email = userEmail;
                currentUser.LastName = userLastName;
                currentUser.FirstName = userFirstName;
                currentUser.MiddleName = userMiddleName;
                currentUser.PhoneNumber = userPhoneNumber;

                currentUser.RoleRef = newRole.RoleId;
                currentUser.RoleRefNavigation = newRole;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Помилка при оновленні даних користувача: {ex.Message}");
            }
        }
    }
}
