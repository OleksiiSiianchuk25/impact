// Імпорт необхідних бібліотек
using NUnit.Framework;
using Moq;
using EfCore.context;
using EfCore.entity;
using EFCore.service.impl;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using EfCore.service.impl;

namespace TestProject.service.impl
{
    [TestFixture]
    public class RequestCategoryServiceImplTest
    {
        [Test]
        public void TestGetAllCategories_ReturnsListOfCategories()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var requestCategoryService = new RequestCategoryServiceImpl(mockContext.Object);

            var categories = new List<RequestCategory>
            {
                new RequestCategory { CategoryId = 1, CategoryName = "Category 1" },
                new RequestCategory { CategoryId = 2, CategoryName = "Category 2" },
                new RequestCategory { CategoryId = 3, CategoryName = "Category 3" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<RequestCategory>>();
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.Provider).Returns(categories.Provider);
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.Expression).Returns(categories.Expression);
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.ElementType).Returns(categories.ElementType);
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());

            mockContext.Setup(c => c.RequestCategories).Returns(mockSet.Object);

            // Act
            var result = requestCategoryService.GetAllCategories();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void TestGetCategoryById_CategoryExists_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new RequestCategory { CategoryId = categoryId, CategoryName = "Category1" };

            var dbContextMock = new Mock<ImpactDbContext>();
            var dbSetMock = MockDbSet.CreateDbSetMock(new List<RequestCategory> { category });

            dbContextMock.Setup(c => c.RequestCategories).Returns(dbSetMock.Object);

            dbSetMock.Setup(d => d.Find(It.IsAny<object[]>())).Returns((object[] keyValues) =>
            {
                var id = (int)keyValues[0];
                return dbSetMock.Object.FirstOrDefault(c => c.CategoryId == id);
            });

            var categoryService = new RequestCategoryServiceImpl(dbContextMock.Object);

            // Act
            var result = categoryService.GetCategoryById(categoryId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(categoryId, result.CategoryId);
            Assert.AreEqual(category.CategoryName, result.CategoryName);
        }

        [Test]
        public void TestGetCategoryById_CategoryNotFound_ThrowsException()
        {
            // Arrange
            var mockContext = new Mock<ImpactDbContext>();
            var requestCategoryService = new RequestCategoryServiceImpl(mockContext.Object);

            var categories = new List<RequestCategory>().AsQueryable();
            var mockSet = new Mock<DbSet<RequestCategory>>();
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.Provider).Returns(categories.Provider);
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.Expression).Returns(categories.Expression);
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.ElementType).Returns(categories.ElementType);
            mockSet.As<IQueryable<RequestCategory>>().Setup(m => m.GetEnumerator()).Returns(categories.GetEnumerator());

            mockContext.Setup(c => c.RequestCategories).Returns(mockSet.Object);

            // Act & Assert
            Assert.Throws<ApplicationException>(() => requestCategoryService.GetCategoryById(1));
        }
    }
}
