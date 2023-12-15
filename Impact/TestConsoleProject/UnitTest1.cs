using ImpactConsole;
using Npgsql;

namespace TestConsoleProject
{
    [TestFixture]
    public class Tests
    {
        private string connectionString;

        [SetUp]
        public void Setup()
        {
            connectionString = String.Format("Server={0};Port={1};" +
                "User Id={2};Password={3};Database={4};",
                "ep-empty-recipe-96792924.eu-central-1.aws.neon.tech", 5432,
                "sijanchuk", "nN8hVXe1pILY", "impact-db");
        }

        [Test]
        public void Connection()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                }
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Test]
        public void TestGetRandomUserId_ShouldReturnValidUserId()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var result = Program.GetRandomUserId(connection);

                Assert.Greater(result, 0);
            }
        }

        [Test]
        public void TestGetRandomRequestId_ShouldReturnValidRequestId()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var result = Program.GetRandomRequestId(connection);

                Assert.Greater(result, 0);
            }
        }

        [Test]
        public void TestGetRandomCategoryId_ShouldReturnValidCategoryId()
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var result = Program.GetRandomCategoryId(connection);

                Assert.Greater(result, 0);
            }
        }
    }
}