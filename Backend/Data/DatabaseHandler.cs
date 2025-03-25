using System;
using Microsoft.Data.Sqlite;

namespace Backend.Data
{
    public class DatabaseHandler
    {
        private readonly string connectionString;

        public DatabaseHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Restaurants (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Cuisine TEXT NOT NULL,
                        Distance INTEGER NOT NULL,
                        PriceRange TEXT NOT NULL
                    );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("✅ Table created or already exists.");
            }
        }

        public void InsertRestaurants()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // ✅ Kolla om data redan finns
                var checkQuery = "SELECT COUNT(*) FROM Restaurants";
                using (var checkCmd = new SqliteCommand(checkQuery, connection))
                {
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        Console.WriteLine("ℹ️ Data already exists. Skipping insert.");
                        return;
                    }
                }

                // ✅ Lägg till restauranger om tabellen är tom
                string insertQuery = @"
                    INSERT INTO Restaurants (Name, Cuisine, Distance, PriceRange) VALUES 
                    ('Marrakesh', 'Moroccan Buffet', 100, '100-200kr'),
                    ('Parked Salad Bar', 'Salad', 100, '100-200kr'),
                    ('Smeden Mathall', 'Foodcourt', 100, '100-200kr'),
                    ('Klå Fann Thai', 'Thai', 350, '100-200kr'),
                    ('Babas', 'Burgers', 200, '100-200kr'),
                    ('Pocket in the Park', 'Husman/Bistro', 200, '100-200kr'),
                    ('Le Kebab', 'Kebab', 300, '100-200kr'),
                    ('Itamae', 'Sushi', 300, '100-200kr'),
                    ('Café Volta', 'Modern European', 200, '100-200kr');";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("✅ Restaurants inserted successfully.");
            }
        }

        public void DisplayRestaurants()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            string selectQuery = "SELECT * FROM Restaurants";

            using var command = new SqliteCommand(selectQuery, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("Id"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string cuisine = reader.GetString(reader.GetOrdinal("Cuisine"));
                int distance = reader.GetInt32(reader.GetOrdinal("Distance"));
                string priceRange = reader.GetString(reader.GetOrdinal("PriceRange"));

                Console.WriteLine($"{id}: {name} - {cuisine} ({distance} m) - Price: {priceRange}");
            }
        }
    }
}
