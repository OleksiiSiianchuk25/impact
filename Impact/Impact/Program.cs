using System;
using System.Data;
using Npgsql;
using Bogus;


namespace Impact
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = String.Format("Server={0};Port={1};" +
                "User Id={2};Password={3};Database={4};",
                "ep-empty-recipe-96792924.eu-central-1.aws.neon.tech", 5432,
                "sijanchuk", "nN8hVXe1pILY", "impact-db");

            var faker = new Bogus.Faker();

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Successfully connected to PostgreSQL.");

                // Заповнення таблиці roles
                for (int i = 0; i < 30; i++)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO roles (role_name) VALUES (@role_name)";
                        command.Parameters.AddWithValue("role_name", faker.Random.Word());
                        command.ExecuteNonQuery();
                    }
                }

                // Заповнення таблиці users
                for (int i = 0; i < 30; i++)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO users (first_name, last_name, email, phone_number, password, role_ref) VALUES (@first_name, @last_name, @email, @phone_number, @password, @role_ref)";
                        command.Parameters.AddWithValue("first_name", faker.Name.FirstName());
                        command.Parameters.AddWithValue("last_name", faker.Name.LastName());
                        command.Parameters.AddWithValue("email", faker.Internet.Email());
                        string phone = faker.Phone.PhoneNumber();
                        if (phone.Length > 20)
                        {
                            phone = phone.Substring(0, 20);
                        }
                        command.Parameters.AddWithValue("phone_number", phone);
                        command.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(faker.Lorem.Sentence()));
                        command.Parameters.AddWithValue("role_ref", faker.Random.Number(1, 4)); 
                        command.ExecuteNonQuery();
                    }
                }

                // Заповнення таблиці request_roles
                for (int i = 0; i < 30; i++)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO request_roles (role_name) VALUES (@role_name)";
                        command.Parameters.AddWithValue("role_name", faker.Random.Word());
                        command.ExecuteNonQuery();
                    }
                }

                // Заповнення таблиці requests
                for (int i = 0; i < 30; i++)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO requests (request_name, description, location, creator_user_ref, contact_phone, contact_email, role_ref) VALUES (@request_name, @description, @location, @creator_user_ref, @contact_phone, @contact_email, @role_ref)";
                        string requestName = faker.Lorem.Sentence();
                        if (requestName.Length > 20)
                        {
                            requestName = requestName.Substring(0, 20);
                        }
                        command.Parameters.AddWithValue("request_name", requestName);
                        command.Parameters.AddWithValue("description", faker.Lorem.Paragraph());
                        command.Parameters.AddWithValue("location", faker.Address.City());
                        command.Parameters.AddWithValue("creator_user_ref", GetRandomUserId(connection));
                        string phone = faker.Phone.PhoneNumber();
                        if (phone.Length > 20)
                        {
                            phone = phone.Substring(0, 20);
                        }
                        command.Parameters.AddWithValue("contact_phone", phone);
                        command.Parameters.AddWithValue("contact_email", faker.Internet.Email());
                        command.Parameters.AddWithValue("role_ref", faker.Random.Number(1, 4)); 
                        command.ExecuteNonQuery();
                    }
                }

                // Заповнення таблиці request_categories
                for (int i = 0; i < 30; i++)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO request_categories (category_name) VALUES (@category_name)";
                        command.Parameters.AddWithValue("category_name", faker.Random.Word());
                        command.ExecuteNonQuery();
                    }
                }

                // Заповнення таблиці request_categories_mapping
                for (int i = 0; i < 30; i++)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO request_categories_mapping (request_id, category_id) VALUES (@request_id, @category_id)";
                        command.Parameters.AddWithValue("request_id", GetRandomRequestId(connection));
                        command.Parameters.AddWithValue("category_id", GetRandomCategoryId(connection));
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
                Console.WriteLine("Connection Closed.");
            }

            Console.ReadLine();
        }

        public static int GetRandomUserId(NpgsqlConnection conn)
        {
            string query = "SELECT user_id FROM users ORDER BY random() LIMIT 1";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        public static int GetRandomRequestId(NpgsqlConnection conn)
        {
            string query = "SELECT request_id FROM requests ORDER BY random() LIMIT 1";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }

        public static int GetRandomCategoryId(NpgsqlConnection conn)
        {
            string query = "SELECT category_id FROM request_categories ORDER BY random() LIMIT 1";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }


        public static void PrintDataTableInColumn(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn column in table.Columns)
                {
                    Console.WriteLine($"{column.ColumnName}: {row[column]}");
                    Console.WriteLine("---------------------------------------------------------------------------------------");
                }
                Console.WriteLine();
            }
        }
    }
}
