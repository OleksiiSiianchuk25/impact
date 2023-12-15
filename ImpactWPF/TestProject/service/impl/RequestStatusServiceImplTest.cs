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
    public class RequestStatusServiceImplTest
    {
        [Test]
        public void TestGetActiveRequestStatus_ReturnsActiveRequestStatus()
        {
            // Arrange
            var statusId = 1;
            var status = new RequestStatus { StatusId = statusId, StatusName = "Active" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<RequestStatus> { status });

            dbContextMock.Setup(c => c.RequestStatuses).Returns(dbSetMock.Object);

            dbSetMock.Setup(d => d.Find(It.IsAny<object[]>())).Returns((object[] keyValues) =>
            {
                var id = (int)keyValues[0];
                return dbSetMock.Object.FirstOrDefault(s => s.StatusId == id);
            });

            var statusService = new RequestStatusServiceImpl(dbContextMock.Object);

            // Act
            var result = statusService.GetActiveRequestStatus();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(statusId, result.StatusId);
            Assert.AreEqual(status.StatusName, result.StatusName);
        }

        [Test]
        public void TestGetInactiveRequestStatus_ReturnsInactiveRequestStatus()
        {
            // Arrange
            var statusId = 2;
            var status = new RequestStatus { StatusId = statusId, StatusName = "Inactive" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<RequestStatus> { status });

            dbContextMock.Setup(c => c.RequestStatuses).Returns(dbSetMock.Object);

            dbSetMock.Setup(d => d.Find(It.IsAny<object[]>())).Returns((object[] keyValues) =>
            {
                var id = (int)keyValues[0];
                return dbSetMock.Object.FirstOrDefault(s => s.StatusId == id);
            });

            var statusService = new RequestStatusServiceImpl(dbContextMock.Object);

            // Act
            var result = statusService.GetInactiveRequestStatus();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(statusId, result.StatusId);
            Assert.AreEqual(status.StatusName, result.StatusName);
        }
    }
}
