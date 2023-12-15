using EfCore.context;
using EfCore.entity;
using EFCore.service.impl;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.service.impl
{
    [TestFixture]
    public class AuthServiceImplTest
    {
        [Test]
        public void TestAuthenticateUser_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var authService = new AuthServiceImpl(mockContext.Object);

            var user = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                RoleRef = 1
            };

            // Setup IQueryable<User> instead of DbSet<User>
            var users = new List<User> { user }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            bool result = authService.AuthenticateUser("john.doe@example.com", "password123");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TestAuthenticateUser_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var authService = new AuthServiceImpl(mockContext.Object);

            var user = new User
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                RoleRef = 1
            };

            var users = new List<User> { user }.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                   .Returns((object[] keyValues) => users.FirstOrDefault(u => u.UserId == (int)keyValues[0]));

            // Act
            bool result = authService.AuthenticateUser("john.doe@example.com", "wrongpassword");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void TestAuthenticateUser_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var authService = new AuthServiceImpl(mockContext.Object);

            var users = new List<User>().AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            bool result = authService.AuthenticateUser("nonexistent@example.com", "password123");

            // Assert
            Assert.IsFalse(result);
        }


    }
}
