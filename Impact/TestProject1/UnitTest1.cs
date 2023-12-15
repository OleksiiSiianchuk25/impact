using Microsoft.VisualStudio.TestPlatform.TestHost;
using Npgsql;
using System.Data;

namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        private string connectionString;

        [SetUp]
        public void Setup()
        {
            // Ось рядок підключення до бази даних. Будь ласка, замініть його на свій рядок підключення.
            connectionString = "Server=ep-empty-recipe-96792924.eu-central-1.aws.neon.tech;Port=5432;User Id=sijanchuk;Password=nN8hVXe1pILY;Database=impact-db;";
        }

        [Test]
        public void TestMainMethod_SuccessfullyConnected()
        {
            // Arrange
            var program = new Program();

            // Act
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Assert
                Assert.IsTrue(connection.State == ConnectionState.Open);
            }
        }

        [Test]
        public void TestGetRandomUserId_ShouldReturnValidUserId()
        {
            // Arrange
            var program = new Program();

            // Act
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var result = program.GetRandomUserId(connection);

                // Assert
                Assert.Greater(result, 0);
            }
        }

        [Test]
        public void TestGetRandomRequestId_ShouldReturnValidRequestId()
        {
            // Arrange
            var program = new Program();

            // Act
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var result = Program.GetRandomRequestId(connection);

                // Assert
                Assert.Greater(result, 0);
            }
        }

        [Test]
        public void TestGetRandomCategoryId_ShouldReturnValidCategoryId()
        {
            // Arrange
            var program = new Program();

            // Act
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var result = program.GetRandomCategoryId(connection);

                // Assert
                Assert.Greater(result, 0);
            }
        }

    }
}