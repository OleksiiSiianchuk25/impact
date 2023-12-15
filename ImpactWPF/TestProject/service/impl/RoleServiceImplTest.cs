using EfCore.context;
using EfCore.entity;
using EfCore.service.impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.service.impl
{
    [TestFixture]
    public class RoleServiceImplTest
    {

        private RoleServiceImpl roleService;
        private ImpactDbContext context;

        [SetUp]
        public void Setup()
        {
            context = new ImpactDbContext();
            roleService = new RoleServiceImpl(context);
        }

        [Test]
        public void GetOrdererRole_ShouldReturnRoleFromContext()
        {
            // Arrange
            var expectedRoleId = 1;
            var expectedRoleName = "ROLE_ORDERER";

            // Act
            var result = roleService.GetOrdererRole();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRoleId, result.RoleId);
            Assert.AreEqual(expectedRoleName, result.RoleName);
        }

        [Test]
        public void GetVolunteerRole_ShouldReturnRoleFromContext()
        {
            // Arrange
            var expectedRoleId = 2;
            var expectedRoleName = "ROLE_VOLUNTEER";

            // Act
            var result = roleService.GetVolunteerRole();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRoleId, result.RoleId);
            Assert.AreEqual(expectedRoleName, result.RoleName);
        }

        [Test]
        public void GetAdminRole_ShouldReturnRoleFromContext()
        {
            // Arrange
            var expectedRoleId = 3;
            var expectedRoleName = "ROLE_ADMIN";

            // Act
            var result = roleService.GetAdminRole();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRoleId, result.RoleId);
            Assert.AreEqual(expectedRoleName, result.RoleName);
        }


    }
}
