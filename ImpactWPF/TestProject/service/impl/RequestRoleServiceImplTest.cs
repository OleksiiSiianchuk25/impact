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
    public class RequestRoleServiceImplTest
    {
        [Test]
        public void TestGetOrderRequestRole_ReturnsOrderRequestRole()
        {
            // Arrange
            var roleId = 1;
            var role = new RequestRole { RoleId = roleId, RoleName = "OrderRequestRole" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<RequestRole> { role });

            dbContextMock.Setup(c => c.RequestRoles).Returns(dbSetMock.Object);

            // Налаштовуємо поведінку Find для повернення ролі, яку ми передали як аргумент
            dbSetMock.Setup(d => d.Find(It.IsAny<object[]>())).Returns((object[] keyValues) =>
            {
                var id = (int)keyValues[0];
                return dbSetMock.Object.FirstOrDefault(r => r.RoleId == id);
            });

            var roleService = new RequestRoleServiceImpl(dbContextMock.Object);

            // Act
            var result = roleService.GetOrderRequestRole();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roleId, result.RoleId);
            Assert.AreEqual(role.RoleName, result.RoleName);
        }

        [Test]
        public void TestGetPropositionRequestRole_ReturnsPropositionRequestRole()
        {
            // Arrange
            var roleId = 2;
            var role = new RequestRole { RoleId = roleId, RoleName = "PropositionRequestRole" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<RequestRole> { role });

            dbContextMock.Setup(c => c.RequestRoles).Returns(dbSetMock.Object);

            // Налаштовуємо поведінку Find для повернення ролі, яку ми передали як аргумент
            dbSetMock.Setup(d => d.Find(It.IsAny<object[]>())).Returns((object[] keyValues) =>
            {
                var id = (int)keyValues[0];
                return dbSetMock.Object.FirstOrDefault(r => r.RoleId == id);
            });

            var roleService = new RequestRoleServiceImpl(dbContextMock.Object);

            // Act
            var result = roleService.GetPropositionRequestRole();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(roleId, result.RoleId);
            Assert.AreEqual(role.RoleName, result.RoleName);
        }
    }
}
