using EfCore.context;
using EfCore.dto;
using EfCore.entity;
using EfCore.service.impl;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.service.impl
{
    [TestFixture]
    public class UserServiceImplTest
    {
        [Test]
        public void TestGetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var users = new List<User>
            {
                new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123456789", Password = "hashedPassword", RoleRef = 1 },
                new User { UserId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "987654321", Password = "hashedPassword", RoleRef = 2 }
                // Add more users if needed
            };

            var mockSet = MockDbSet.CreateDbSetMock(users);
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            var result = userService.GetAllUsers();

            // Assert
            Assert.AreEqual(users.Count, result.Count);
            CollectionAssert.AreEqual(users, result);
        }

        [Test]
        public void TestDeleteUserById_UserExists_UserRemoved()
        {
            // Arrange
            var userId = 1;
            var mockContext = new Mock<ImpactDbContext>();
            var mockSet = new Mock<DbSet<User>>();

            var userToDelete = new User
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            mockSet.Setup(m => m.Find(userId)).Returns(userToDelete);
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var userService = new UserServiceImpl(mockContext.Object);

            // Act
            userService.DeleteUserById(userId);

            // Assert
            mockSet.Verify(m => m.Remove(userToDelete), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void TestDeleteUserById_UserDoesNotExist_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var mockContext = new Mock<ImpactDbContext>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.Setup(m => m.Find(userId)).Returns((User)null); // User not found
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var userService = new UserServiceImpl(mockContext.Object);

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.DeleteUserById(userId));
            mockSet.Verify(m => m.Remove(It.IsAny<User>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Test]
        public void TestRegisterUser_PasswordMismatch_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var userDTO = new UserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "password123",
                ConfirmPassword = "mismatchedpassword",
                RoleId = 1
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => userService.RegisterUser(userDTO));
        }

        [Test]
        public void TestRegisterUser_PasswordMatch_AddsUserToContext()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var mockSet = new Mock<DbSet<User>>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var userService = new UserServiceImpl(mockContext.Object);

            var userDTO = new UserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Middle",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "password123",
                ConfirmPassword = "password123",
                RoleId = 1
            };

            var userRole = new Role { RoleId = 1, RoleName = "ROLE_USER" };

            // Set up behavior for RoleRefNavigation
            mockContext.Setup(c => c.Roles.Find(userDTO.RoleId)).Returns(userRole);

            // Act
            userService.RegisterUser(userDTO);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void TestGetOrderers_ReturnsOrderers()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>();

            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            var ordererRoleId = 1; // Assuming 1 is the role ID for "Orderer"

            var ordererRole = new Role { RoleId = ordererRoleId, RoleName = "ROLE_ORDERER" };

            var orderers = new List<User>
            {
                new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "123456789", Password = "hashedPassword", RoleRef = ordererRoleId },
                new User { UserId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", PhoneNumber = "987654321", Password = "hashedPassword", RoleRef = ordererRoleId },
                // Add more orderers if needed
            };

            var mockSet = MockDbSet.CreateDbSetMock(orderers);
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Set up behavior for GetOrdererRole in the mockRoleService
            mockRoleService.Setup(r => r.GetOrdererRole()).Returns(ordererRole);

            // Act
            var result = userService.GetOrderers();

            // Assert
            Assert.AreEqual(orderers.Count, result.Count);
            CollectionAssert.AreEqual(orderers, result);
        }

        [Test]
        public void TestGetVolunteers_ReturnsVolunteers()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>();

            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            var volunteerRoleId = 2; // Assuming 2 is the role ID for "Volunteer"

            var volunteerRole = new Role { RoleId = volunteerRoleId, RoleName = "ROLE_VOLUNTEER" };

            var volunteers = new List<User>
            {
                new User { UserId = 1, FirstName = "Volunteer1", LastName = "Doe", Email = "volunteer1@example.com", PhoneNumber = "123456789", Password = "hashedPassword", RoleRef = volunteerRoleId },
                new User { UserId = 2, FirstName = "Volunteer2", LastName = "Doe", Email = "volunteer2@example.com", PhoneNumber = "987654321", Password = "hashedPassword", RoleRef = volunteerRoleId },
                // Add more volunteers if needed
            };

            var mockSet = MockDbSet.CreateDbSetMock(volunteers);
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Set up behavior for GetVolunteerRole in the mockRoleService
            mockRoleService.Setup(r => r.GetVolunteerRole()).Returns(volunteerRole);

            // Act
            var result = userService.GetVolunteers();

            // Assert
            Assert.AreEqual(volunteers.Count, result.Count);
            CollectionAssert.AreEqual(volunteers, result);
        }

        [Test]
        public void TestGetUserRoleByEmail_UserExists_ReturnsUserRole()
        {
            // Arrange
            var userEmail = "john.doe@example.com";
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var userRole = new Role { RoleId = 1, RoleName = "ROLE_USER" };
            var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = userEmail, PhoneNumber = "123456789", Password = "hashedPassword", RoleRef = userRole.RoleId };

            // Create a mock DbSet<User>
            var mockSet = MockDbSet.CreateDbSetMock(new List<User> { user }.AsQueryable());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            mockContext.Setup(c => c.Roles.Find(userRole.RoleId)).Returns(userRole);

            // Act
            var result = userService.GetUserRoleByEmail(userEmail);

            // Assert
            Assert.AreEqual(userRole, result);
        }


        [Test]
        public void TestGetUserRoleByEmail_UserDoesNotExist_ThrowsException()
        {
            // Arrange
            var userEmail = "nonexistent.user@example.com";
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            mockContext.Setup(c => c.Users)
                       .Returns(MockDbSet.CreateDbSetMock(new List<User>()).Object); // No users in the context

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.GetUserRoleByEmail(userEmail));
        }


        [Test]
        public void TestGetUserById_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var user = new User
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            var mockSet = MockDbSet.CreateDbSetMock(new List<User> { user });
            mockContext.Setup(c => c.Users.Find(userId)).Returns(user);

            // Act
            var result = userService.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(user.UserId, result.UserId);
            Assert.AreEqual(user.FirstName, result.FirstName);
            Assert.AreEqual(user.LastName, result.LastName);
            Assert.AreEqual(user.Email, result.Email);
        }


        [Test]
        public void TestGetUserById_UserDoesNotExist_ThrowsException()
        {
            // Arrange
            var userId = 1;
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            // Create an empty queryable collection for Users
            var usersQueryable = new List<User>().AsQueryable();

            // Use DbSetExtensions to create a DbSet mock
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(usersQueryable.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(usersQueryable.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(usersQueryable.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(usersQueryable.GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.GetUserById(userId));
        }

        [Test]
        public void TestChangePassword_UserExists_ChangesPassword()
        {
            // Arrange
            var userEmail = "john.doe@example.com";
            var newPassword = "newpassword";
            var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = userEmail, PhoneNumber = "123456789", RoleRef = 1, Password = "oldpassword" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<User> { user });

            dbContextMock.Setup(c => c.Users).Returns(dbSetMock.Object);

            var userService = new UserServiceImpl(dbContextMock.Object);

            // Act
            userService.ChangePassword(userEmail, newPassword);

            // Assert
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(newPassword, user.Password));
            dbContextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void TestUpdateUserPassword_UserExists_UpdatesUserPassword()
        {
            // Arrange
            var userEmail = "john.doe@example.com";
            var user = new User { UserId = 1, FirstName = "John", LastName = "Doe", Email = userEmail, PhoneNumber = "123456789", RoleRef = 1, Password = "oldpassword" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<User> { user });

            dbContextMock.Setup(c => c.Users).Returns(dbSetMock.Object);

            var userService = new UserServiceImpl(dbContextMock.Object);

            // Act
            userService.UpdateUserPassword(user, "newpassword");

            // Assert
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify("newpassword", user.Password));
            dbContextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void TestUpdateUserData_ValidData_UserDataUpdated()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            var newRole = new Role { RoleId = 2, RoleName = "ROLE_ADMIN" };

            mockContext.Setup(c => c.Roles).Returns(MockDbSet.CreateDbSetMock(new List<Role> { newRole }.AsQueryable()).Object);

            // Act
            userService.UpdateUserData(currentUser, "new.email@example.com", "NewLastName", "NewFirstName", "NewMiddleName", "987654321", "Адмін");

            // Assert
            Assert.AreEqual("new.email@example.com", currentUser.Email);
            Assert.AreEqual("NewLastName", currentUser.LastName);
            Assert.AreEqual("NewFirstName", currentUser.FirstName);
            Assert.AreEqual("NewMiddleName", currentUser.MiddleName);
            Assert.AreEqual("987654321", currentUser.PhoneNumber);
            Assert.AreEqual(newRole.RoleId, currentUser.RoleRef);
            Assert.AreEqual(newRole, currentUser.RoleRefNavigation);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }



        [Test]
        public void TestUpdateUserData_InvalidRole_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.UpdateUserData(currentUser, "new.email@example.com", "NewLastName", "NewFirstName", "NewMiddleName", "987654321", "InvalidRole"));
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Test]
        public void TestUpdateUserPassword_PasswordUpdated()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            var newPassword = "newPassword123";

            // Act
            userService.UpdateUserPassword(currentUser, newPassword);

            // Assert
            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(newPassword, currentUser.Password));
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }


        [Test]
        public void TestAdminUpdateUserData_ValidData_UserDataUpdated()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            var roles = new List<Role>
            {
                new Role { RoleId = 2, RoleName = "ROLE_ADMIN" },
            };

            mockContext.Setup(c => c.Roles).Returns(MockDbSet.CreateDbSetMock(roles).Object);

            // Act
            userService.AdminUpdateUserData(currentUser, "new.email@example.com", "NewLastName", "NewFirstName", "NewMiddleName", "987654321", "Адмін");

            // Assert
            Assert.AreEqual("new.email@example.com", currentUser.Email);
            Assert.AreEqual("NewLastName", currentUser.LastName);
            Assert.AreEqual("NewFirstName", currentUser.FirstName);
            Assert.AreEqual("NewMiddleName", currentUser.MiddleName);
            Assert.AreEqual("987654321", currentUser.PhoneNumber);
            Assert.AreEqual(2, currentUser.RoleRef);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void TestAdminUpdateUserData_InvalidRole_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.AdminUpdateUserData(currentUser, "new.email@example.com", "NewLastName", "NewFirstName", "NewMiddleName", "987654321", "InvalidRole"));
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Test]
        public void TestAdminUpdateUserData_UserNotFound_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.AdminUpdateUserData(currentUser, "new.email@example.com", "NewLastName", "NewFirstName", "NewMiddleName", "987654321", "Адмін"));
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Test]
        public void TestAdminUpdateUserData_NullRole_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var userService = new UserServiceImpl(mockContext.Object);

            var currentUser = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = "hashedPassword",
                RoleRef = 1
            };

            // Act & Assert
            Assert.Throws<ApplicationException>(() => userService.AdminUpdateUserData(currentUser, "new.email@example.com", "NewLastName", "NewFirstName", "NewMiddleName", "987654321", null));
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Test]
        public void TestGetOrderers_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>();

            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            var ordererRoleId = 1; // Assuming 1 is the role ID for "Orderer"

            var ordererRole = new Role { RoleId = ordererRoleId, RoleName = "ROLE_ORDERER" };

            var mockSet = MockDbSet.CreateDbSetMock(new List<User>());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Set up behavior for GetOrdererRole in the mockRoleService
            mockRoleService.Setup(r => r.GetOrdererRole()).Returns(ordererRole);

            // Act
            var result = userService.GetOrderers();

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void TestGetVolunteers_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var mockRoleService = new Mock<RoleServiceImpl>();

            var userService = new UserServiceImpl(mockContext.Object, mockRoleService.Object);

            var volunteerRoleId = 2; // Assuming 2 is the role ID for "Volunteer"

            var volunteerRole = new Role { RoleId = volunteerRoleId, RoleName = "ROLE_VOLUNTEER" };

            var mockSet = MockDbSet.CreateDbSetMock(new List<User>());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Set up behavior for GetVolunteerRole in the mockRoleService
            mockRoleService.Setup(r => r.GetVolunteerRole()).Returns(volunteerRole);

            // Act
            var result = userService.GetVolunteers();

            // Assert
            Assert.IsEmpty(result);
        }

    }
}
