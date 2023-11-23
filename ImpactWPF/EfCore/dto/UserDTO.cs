using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore.dto
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int RoleId { get; set; }

        public UserDTO()
        {
        }

        public UserDTO(string firstName, string lastName,
            string? middleName, string email, string phoneNumber,
            string password, string confirmPassword, int roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            ConfirmPassword = confirmPassword;
            RoleId = roleId;
        }
    }
}
