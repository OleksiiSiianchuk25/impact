using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.service
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void RequestDTO_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var requestDTO = new RequestDTO();

            // Act - No action needed for the default constructor

            // Assert
            Assert.IsNull(requestDTO.RequestName);
            Assert.IsNull(requestDTO.Description);
            Assert.IsNull(requestDTO.ContactPhone);
            Assert.IsNull(requestDTO.ContactEmail);
            Assert.IsNull(requestDTO.Location);
            Assert.IsNull(requestDTO.CreatorUserRef);
            Assert.AreEqual(0, requestDTO.RoleRef);
            Assert.IsNotNull(requestDTO.Categories);
            Assert.IsEmpty(requestDTO.Categories);
        }

        [Test]
        public void RequestDTO_ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string requestName = "TestRequest";
            string description = "TestDescription";
            string contactPhone = "1234567890";
            string contactEmail = "test@example.com";
            string location = "TestLocation";
            int? creatorUserRef = 1;
            int roleRef = 2;
            ICollection<int> categories = new List<int> { 1, 2, 3 };

            // Act
            var requestDTO = new RequestDTO(requestName, description, contactPhone, contactEmail, location, creatorUserRef, roleRef, categories);

            // Assert
            Assert.AreEqual(requestName, requestDTO.RequestName);
            Assert.AreEqual(description, requestDTO.Description);
            Assert.AreEqual(contactPhone, requestDTO.ContactPhone);
            Assert.AreEqual(contactEmail, requestDTO.ContactEmail);
            Assert.AreEqual(location, requestDTO.Location);
            Assert.AreEqual(creatorUserRef, requestDTO.CreatorUserRef);
            Assert.AreEqual(roleRef, requestDTO.RoleRef);
            Assert.IsNotNull(requestDTO.Categories);
            Assert.AreEqual(categories.Count, requestDTO.Categories.Count);
        }

        [Test]
        public void UserDTO_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var userDTO = new UserDTO();

            // Act - No action needed for the default constructor

            // Assert
            Assert.IsNull(userDTO.FirstName);
            Assert.IsNull(userDTO.LastName);
            Assert.IsNull(userDTO.MiddleName);
            Assert.IsNull(userDTO.Email);
            Assert.IsNull(userDTO.PhoneNumber);
            Assert.IsNull(userDTO.Password);
            Assert.IsNull(userDTO.ConfirmPassword);
            Assert.AreEqual(0, userDTO.RoleId);
        }

        [Test]
        public void UserDTO_ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string firstName = "John";
            string lastName = "Doe";
            string middleName = "M";
            string email = "john.doe@example.com";
            string phoneNumber = "1234567890";
            string password = "password";
            string confirmPassword = "password";
            int roleId = 1;

            // Act
            var userDTO = new UserDTO(firstName, lastName, middleName, email, phoneNumber, password, confirmPassword, roleId);

            // Assert
            Assert.AreEqual(firstName, userDTO.FirstName);
            Assert.AreEqual(lastName, userDTO.LastName);
            Assert.AreEqual(middleName, userDTO.MiddleName);
            Assert.AreEqual(email, userDTO.Email);
            Assert.AreEqual(phoneNumber, userDTO.PhoneNumber);
            Assert.AreEqual(password, userDTO.Password);
            Assert.AreEqual(confirmPassword, userDTO.ConfirmPassword);
            Assert.AreEqual(roleId, userDTO.RoleId);
        }

        [Test]
        public void UserUpdateDTO_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var userUpdateDTO = new UserUpdateDTO();

            // Act - No action needed for the default constructor

            // Assert
            Assert.IsNull(userUpdateDTO.FirstName);
            Assert.IsNull(userUpdateDTO.LastName);
            Assert.IsNull(userUpdateDTO.MiddleName);
            Assert.IsNull(userUpdateDTO.Email);
            Assert.IsNull(userUpdateDTO.PhoneNumber);
            Assert.AreEqual(0, userUpdateDTO.RoleId);
        }

        [Test]
        public void UserUpdateDTO_ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string firstName = "John";
            string lastName = "Doe";
            string middleName = "M";
            string email = "john.doe@example.com";
            string phoneNumber = "1234567890";
            int roleId = 1;

            // Act
            var userUpdateDTO = new UserUpdateDTO(firstName, lastName, middleName, email, phoneNumber, roleId);

            // Assert
            Assert.AreEqual(firstName, userUpdateDTO.FirstName);
            Assert.AreEqual(lastName, userUpdateDTO.LastName);
            Assert.AreEqual(middleName, userUpdateDTO.MiddleName);
            Assert.AreEqual(email, userUpdateDTO.Email);
            Assert.AreEqual(phoneNumber, userUpdateDTO.PhoneNumber);
            Assert.AreEqual(roleId, userUpdateDTO.RoleId);
        }

        [Test]
        public void Request_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var request = new Request();

            // Act - No action needed for the default constructor

            // Assert
            Assert.AreEqual(0, request.RequestId);
            Assert.IsNull(request.RequestName);
            Assert.IsNull(request.Description);
            Assert.IsNull(request.Location);
            Assert.IsNull(request.CreatorUserRef);
            Assert.IsNull(request.ContactPhone);
            Assert.IsNull(request.ContactEmail);
            Assert.AreEqual(0, request.RoleRef);
            Assert.IsNull(request.RequestStatusId);
            Assert.IsNull(request.CreatedAt);
            Assert.IsNull(request.CreatorUserRefNavigation); // Adjusted this line
            Assert.IsNull(request.RequestStatus);
            Assert.IsNull(request.RoleRefNavigation);
            Assert.IsEmpty(request.Categories);
        }


        [Test]
        public void Request_ParameterizedConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            string requestName = "Test Request";
            string description = "Test Description";
            string location = "Test Location";
            int creatorUserRef = 1;
            string contactPhone = "1234567890";
            string contactEmail = "test@example.com";
            int roleRef = 2;
            int requestStatusId = 3;
            DateTime createdAt = DateTime.Now;
            var creatorUserRefNavigation = new User();
            var requestStatus = new RequestStatus();
            var roleRefNavigation = new RequestRole();
            var categories = new List<RequestCategory> { new RequestCategory() };

            // Act
            var request = new Request
            {
                RequestName = requestName,
                Description = description,
                Location = location,
                CreatorUserRef = creatorUserRef,
                ContactPhone = contactPhone,
                ContactEmail = contactEmail,
                RoleRef = roleRef,
                RequestStatusId = requestStatusId,
                CreatedAt = createdAt,
                CreatorUserRefNavigation = creatorUserRefNavigation,
                RequestStatus = requestStatus,
                RoleRefNavigation = roleRefNavigation,
                Categories = categories
            };

            // Assert
            Assert.AreEqual(0, request.RequestId); // Assuming that RequestId is auto-generated in the database
            Assert.AreEqual(requestName, request.RequestName);
            Assert.AreEqual(description, request.Description);
            Assert.AreEqual(location, request.Location);
            Assert.AreEqual(creatorUserRef, request.CreatorUserRef);
            Assert.AreEqual(contactPhone, request.ContactPhone);
            Assert.AreEqual(contactEmail, request.ContactEmail);
            Assert.AreEqual(roleRef, request.RoleRef);
            Assert.AreEqual(requestStatusId, request.RequestStatusId);
            Assert.AreEqual(createdAt, request.CreatedAt);
            Assert.AreEqual(creatorUserRefNavigation, request.CreatorUserRefNavigation);
            Assert.AreEqual(requestStatus, request.RequestStatus);
            Assert.AreEqual(roleRefNavigation, request.RoleRefNavigation);
            Assert.AreEqual(categories, request.Categories);
        }

        [Test]
        public void RequestCategory_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var requestCategory = new RequestCategory();

            // Act - No action needed for the default constructor

            // Assert
            Assert.AreEqual(0, requestCategory.CategoryId);
            Assert.IsNull(requestCategory.CategoryName);
            Assert.IsNotNull(requestCategory.Requests);
            Assert.IsEmpty(requestCategory.Requests);
        }

        [Test]
        public void RequestRole_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var requestRole = new RequestRole();

            // Act - No action needed for the default constructor

            // Assert
            Assert.AreEqual(0, requestRole.RoleId);
            Assert.IsNull(requestRole.RoleName);
            Assert.IsNotNull(requestRole.Requests);
            Assert.IsEmpty(requestRole.Requests);
        }

        [Test]
        public void RequestStatus_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var requestStatus = new RequestStatus();

            // Act - No action needed for the default constructor

            // Assert
            Assert.AreEqual(0, requestStatus.StatusId);
            Assert.IsNull(requestStatus.StatusName);
            Assert.IsNotNull(requestStatus.Requests);
            Assert.IsEmpty(requestStatus.Requests);
        }

        [Test]
        public void Role_DefaultConstructor_SetsPropertiesToDefaultValues()
        {
            // Arrange
            var role = new Role();

            // Act - No action needed for the default constructor

            // Assert
            Assert.AreEqual(0, role.RoleId);
            Assert.IsNull(role.RoleName);
            Assert.IsNotNull(role.Users);
            Assert.IsEmpty(role.Users);
        }

        [Test]
        public void UserSession_Instance_IsSingleton()
        {
            // Arrange
            var instance1 = UserSession.Instance;
            var instance2 = UserSession.Instance;

            // Act - No action needed for the instance property

            // Assert
            Assert.AreSame(instance1, instance2, "UserSession instance is not a singleton.");
        }

        [Test]
        public void UserSession_Login_SetsUserEmailAndUserRole()
        {
            // Arrange
            var userSession = UserSession.Instance;

            // Act
            userSession.Login("test@example.com", "Admin");

            // Assert
            Assert.AreEqual("test@example.com", userSession.UserEmail);
            Assert.AreEqual("Admin", userSession.UserRole);
        }

        [Test]
        public void UserSession_UpdateRole_UpdatesUserRole()
        {
            // Arrange
            var userSession = UserSession.Instance;
            userSession.Login("test@example.com", "User");

            // Act
            userSession.UpdateRole("Admin");

            // Assert
            Assert.AreEqual("Admin", userSession.UserRole);
        }

        [Test]
        public void UserSession_UpdateUserEmail_UpdatesUserEmail()
        {
            // Arrange
            var userSession = UserSession.Instance;
            userSession.Login("test@example.com", "User");

            // Act
            userSession.UpdateUserEmail("newemail@example.com");

            // Assert
            Assert.AreEqual("newemail@example.com", userSession.UserEmail);
        }

        [Test]
        public void UserSession_Logout_ClearsUserEmailAndUserRole()
        {
            // Arrange
            var userSession = UserSession.Instance;
            userSession.Login("test@example.com", "User");

            // Act
            userSession.Logout();

            // Assert
            Assert.IsNull(userSession.UserEmail);
            Assert.IsNull(userSession.UserRole);
        }

        [Test]
        public void Constructor_ShouldUseNpgsql()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ImpactDbContext>()
                .UseNpgsql("Host=ep-empty-recipe-96792924.eu-central-1.aws.neon.tech;Database=impact-db;Username=sijanchuk;Password=nN8hVXe1pILY")
                .Options;

            // Act
            var dbContext = new ImpactDbContext(options);

            // Assert
            Assert.IsTrue(options.Extensions.Any(e => e.GetType() == typeof(Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal.NpgsqlOptionsExtension)));
        }

        

        [Test]
        public void Request_ShouldHaveDefaultValues()
        {
            // Arrange
            var request = new Request();

            // Assert
            Assert.AreEqual(0, request.RequestId);
            Assert.AreEqual(default(string), request.RequestName);
            Assert.AreEqual(default(string), request.Description);
            Assert.AreEqual(default(string), request.Location);
            Assert.AreEqual(default(int?), request.CreatorUserRef);
            Assert.AreEqual(default(string), request.ContactPhone);
            Assert.AreEqual(default(string), request.ContactEmail);
            Assert.AreEqual(default(int), request.RoleRef);
            Assert.AreEqual(default(int?), request.RequestStatusId);
            Assert.AreEqual(default(DateTime?), request.CreatedAt);
            Assert.AreEqual(default(User), request.CreatorUserRefNavigation);
            Assert.AreEqual(default(RequestStatus), request.RequestStatus);
            Assert.AreEqual(default(RequestRole), request.RoleRefNavigation);
            Assert.IsNotNull(request.Categories);
        }
    }
}
