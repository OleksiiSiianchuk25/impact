using System;
using System.Data;
using Npgsql;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = String.Format("Server={0};Port={1};" +
        "User Id={2};Password={3};Database={4};",
        "ep-empty-recipe-96792924.eu-central-1.aws.neon.tech", 5432,
        "sijanchuk", "nN8hVXe1pILY", "neondb");


        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            ClearTable(connection, "volunteer_history");
            ClearTable(connection, "order_history");
            ClearTable(connection, "proposal_categories");
            ClearTable(connection, "order_categories");
            ClearTable(connection, "categories");
            ClearTable(connection, "orders");
            ClearTable(connection, "proposals");
            ClearTable(connection, "users");
            ClearTable(connection, "roles");

            InsertRandomRolesTable(connection, 50);
            InsertRandomUsers(connection, 50);
            InsertRandomProposals(connection, 50);
            InsertRandomOrders(connection, 50);
            InsertRandomCategories(connection, 10);
            InsertRandomProposalCategories(connection, 50);
            InsertRandomOrderCategories(connection, 50);
            InsertRandomVolunteerHistory(connection, 50);
            InsertRandomOrderHistory(connection, 50);

            DisplayRoles(connection);
            DisplayUsers(connection);
            DisplayProposals(connection);
            DisplayOrders(connection);
            DisplayCategories(connection);
            DisplayProposalCategories(connection);
            DisplayOrderCategories(connection);
            DisplayVolunteerHistory(connection);
            DisplayOrderHistory(connection);
        }
    }
    private static void ClearTable(NpgsqlConnection connection, string tableName)
    {
        using (NpgsqlCommand cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = $"TRUNCATE TABLE {tableName} CASCADE;";
            cmd.ExecuteNonQuery();
        }
    }

    private static void InsertRandomRolesTable(NpgsqlConnection connection, int numberOfRecords)
    {
        for (int i = 0; i < numberOfRecords; i++)
        {
            string roleName = "RoleName" + i;
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;

                cmd.CommandText = "INSERT INTO roles (role_name) VALUES (@RoleName)";
                cmd.Parameters.AddWithValue("@RoleName", roleName);
                    
                cmd.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomUsers(NpgsqlConnection connection, int numUsers)
    {
        Random random = new Random();

        for (int i = 0; i < numUsers; i++)
        {
            string firstName = "User" + i;
            string lastName = "LastName" + i;
            string middleName = "MiddleName" + i;
            string email = $"user{i}@example.com";
            string phoneNumber = "+1234567890";

            string query = "INSERT INTO users (first_name, last_name, middle_name, email, phone_number, role_id) " +
                           "VALUES (@FirstName, @LastName, @MiddleName, @Email, @PhoneNumber, @RoleId)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@RoleId", i % 3 + 1);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomProposals(NpgsqlConnection connection, int numProposals)
    {
        Random random = new Random();

        for (int i = 0; i < numProposals; i++)
        {
            string proposalName = "Proposal" + i;
            string description = "Description of proposal " + i;
            string location = "Location " + i;
            int creatorUserId = random.Next(1, 51); 
            string contactPhone = "+1234567890";
            string contactEmail = $"proposal{i}@example.com";

            string query = "INSERT INTO proposals (proposal_name, description, location, creator_user_id, contact_phone, contact_email) " +
                           "VALUES (@ProposalName, @Description, @Location, @CreatorUserId, @ContactPhone, @ContactEmail)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProposalName", proposalName);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Location", location);
                command.Parameters.AddWithValue("@CreatorUserId", creatorUserId);
                command.Parameters.AddWithValue("@ContactPhone", contactPhone);
                command.Parameters.AddWithValue("@ContactEmail", contactEmail);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomOrders(NpgsqlConnection connection, int numOrders)
    {
        Random random = new Random();

        for (int i = 0; i < numOrders; i++)
        {
            string orderName = "Order" + i;
            string description = "Description of order " + i;
            string location = "Location " + i;
            int creatorUserId = random.Next(1, 51); 
            string contactPhone = "+1234567890";
            string contactEmail = $"order{i}@example.com";

            string query = "INSERT INTO orders (order_name, description, location, creator_user_id, contact_phone, contact_email) " +
                           "VALUES (@OrderName, @Description, @Location, @CreatorUserId, @ContactPhone, @ContactEmail)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderName", orderName);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Location", location);
                command.Parameters.AddWithValue("@CreatorUserId", creatorUserId);
                command.Parameters.AddWithValue("@ContactPhone", contactPhone);
                command.Parameters.AddWithValue("@ContactEmail", contactEmail);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomCategories(NpgsqlConnection connection, int numCategories)
    {
        Random random = new Random();

        for (int i = 0; i < numCategories; i++)
        {
            string categoryName = "Category" + i;

            string query = "INSERT INTO categories (category_name) " +
                           "VALUES (@CategoryName)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", categoryName);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomProposalCategories(NpgsqlConnection connection, int numRecords)
    {
        Random random = new Random();

        for (int i = 0; i < numRecords; i++)
        {
            int proposalId = random.Next(1, 51); 
            int categoryId = random.Next(1, 11);  

            string query = "INSERT INTO proposal_categories (proposal_id, category_id) " +
                           "VALUES (@ProposalId, @CategoryId)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProposalId", proposalId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomOrderCategories(NpgsqlConnection connection, int numRecords)
    {
        Random random = new Random();

        for (int i = 0; i < numRecords; i++)
        {
            int orderId = random.Next(1, 51);      
            int categoryId = random.Next(1, 11);   

            string query = "INSERT INTO order_categories (order_id, category_id) " +
                           "VALUES (@OrderId, @CategoryId)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomVolunteerHistory(NpgsqlConnection connection, int numRecords)
    {
        Random random = new Random();

        for (int i = 0; i < numRecords; i++)
        {
            int volunteerId = random.Next(1, 51);   
            int proposalId = random.Next(1, 51);    

            string query = "INSERT INTO volunteer_history (volunteer_id, proposal_id) " +
                           "VALUES (@VolunteerId, @ProposalId)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VolunteerId", volunteerId);
                command.Parameters.AddWithValue("@ProposalId", proposalId);

                command.ExecuteNonQuery();
            }
        }
    }

    private static void InsertRandomOrderHistory(NpgsqlConnection connection, int numRecords)
    {
        Random random = new Random();

        for (int i = 0; i < numRecords; i++)
        {
            int userId = random.Next(1, 51);        
            int orderId = random.Next(1, 51);       

            string query = "INSERT INTO order_history (user_id, order_id) " +
                           "VALUES (@UserId, @OrderId)";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@OrderId", orderId);

                command.ExecuteNonQuery();
            }
        }
    }

    // Methods for displaying data of tables
    private static void DisplayRoles(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM roles";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Roles Table Data:");

            while (reader.Read())
            {
                int roleId = reader.GetInt32(reader.GetOrdinal("role_id"));
                string roleName = reader.GetString(reader.GetOrdinal("role_name"));

                Console.WriteLine($"RoleID: {roleId}, RoleName: {roleName}");
            }
        }
    }


    private static void DisplayUsers(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM users";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Users Table Data:");

            while (reader.Read())
            {
                int userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                string firstName = reader.GetString(reader.GetOrdinal("first_name"));
                string lastName = reader.GetString(reader.GetOrdinal("last_name"));
                string middleName = reader.GetString(reader.GetOrdinal("middle_name"));
                string email = reader.GetString(reader.GetOrdinal("email"));
                string phoneNumber = reader.GetString(reader.GetOrdinal("phone_number"));
                int role = reader.GetInt32(reader.GetOrdinal("role_id"));

                Console.WriteLine($"UserID: {userId}, FirstName: {firstName}, LastName: {lastName}, MiddleName: {middleName}, Email: {email}, Phone: {phoneNumber}, Role: {role}");
            }
        }
    }

    private static void DisplayProposals(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM proposals";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Proposals Table Data:");

            while (reader.Read())
            {
                int proposalId = reader.GetInt32(reader.GetOrdinal("proposal_id"));
                string proposalName = reader.GetString(reader.GetOrdinal("proposal_name"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                string location = reader.GetString(reader.GetOrdinal("location"));
                int creatorUserId = reader.GetInt32(reader.GetOrdinal("creator_user_id"));
                string contactPhone = reader.GetString(reader.GetOrdinal("contact_phone"));
                string contactEmail = reader.GetString(reader.GetOrdinal("contact_email"));

                Console.WriteLine($"ProposalID: {proposalId}, Name: {proposalName}, Description: {description}, Location: {location}, CreatorUserID: {creatorUserId}, ContactPhone: {contactPhone}, ContactEmail: {contactEmail}");
            }
        }
    }

    private static void DisplayOrders(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM orders";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Orders Table Data:");

            while (reader.Read())
            {
                int orderId = reader.GetInt32(reader.GetOrdinal("order_id"));
                string orderName = reader.GetString(reader.GetOrdinal("order_name"));
                string description = reader.GetString(reader.GetOrdinal("description"));
                string location = reader.GetString(reader.GetOrdinal("location"));
                int creatorUserId = reader.GetInt32(reader.GetOrdinal("creator_user_id"));
                string contactPhone = reader.GetString(reader.GetOrdinal("contact_phone"));
                string contactEmail = reader.GetString(reader.GetOrdinal("contact_email"));

                Console.WriteLine($"OrderID: {orderId}, Name: {orderName}, Description: {description}, Location: {location}, CreatorUserID: {creatorUserId}, ContactPhone: {contactPhone}, ContactEmail: {contactEmail}");
            }
        }
    }

    private static void DisplayCategories(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM categories";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Categories Table Data:");

            while (reader.Read())
            {
                int categoryId = reader.GetInt32(reader.GetOrdinal("category_id"));
                string categoryName = reader.GetString(reader.GetOrdinal("category_name"));

                Console.WriteLine($"CategoryID: {categoryId}, Name: {categoryName}");
            }
        }
    }

    private static void DisplayProposalCategories(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM proposal_categories";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Proposal Categories Table Data:");

            while (reader.Read())
            {
                int proposalCategoryId = reader.GetInt32(reader.GetOrdinal("proposal_category_id"));
                int proposalId = reader.GetInt32(reader.GetOrdinal("proposal_id"));
                int categoryId = reader.GetInt32(reader.GetOrdinal("category_id"));

                Console.WriteLine($"ProposalCategoryID: {proposalCategoryId}, ProposalID: {proposalId}, CategoryID: {categoryId}");
            }
        }
    }

    private static void DisplayOrderCategories(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM order_categories";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Order Categories Table Data:");

            while (reader.Read())
            {
                int orderCategoryId = reader.GetInt32(reader.GetOrdinal("order_category_id"));
                int orderId = reader.GetInt32(reader.GetOrdinal("order_id"));
                int categoryId = reader.GetInt32(reader.GetOrdinal("category_id"));

                Console.WriteLine($"OrderCategoryID: {orderCategoryId}, OrderID: {orderId}, CategoryID: {categoryId}");
            }
        }
    }

    private static void DisplayVolunteerHistory(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM volunteer_history";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Volunteer History Table Data:");

            while (reader.Read())
            {
                int historyId = reader.GetInt32(reader.GetOrdinal("history_id"));
                int volunteerId = reader.GetInt32(reader.GetOrdinal("volunteer_id"));
                int proposalId = reader.GetInt32(reader.GetOrdinal("proposal_id"));

                Console.WriteLine($"HistoryID: {historyId}, VolunteerID: {volunteerId}, ProposalID: {proposalId}");
            }
        }
    }

    private static void DisplayOrderHistory(NpgsqlConnection connection)
    {
        string query = "SELECT * FROM order_history";

        using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
        using (NpgsqlDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("Order History Table Data:");

            while (reader.Read())
            {
                int historyId = reader.GetInt32(reader.GetOrdinal("history_id"));
                int userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                int orderId = reader.GetInt32(reader.GetOrdinal("order_id"));

                Console.WriteLine($"HistoryID: {historyId}, UserID: {userId}, OrderID: {orderId}");
            }
        }
    }

}
