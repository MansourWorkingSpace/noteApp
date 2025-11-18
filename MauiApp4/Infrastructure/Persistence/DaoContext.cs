using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace MauiApp4.Infrastructure.Persistence
{
    // Simple manual SQLite context: manages DB file path and initialization using Microsoft.Data.Sqlite.
    public class DaoContext
    {
        public string DbPath { get; }
        // Id of the current (signed-in) user. Set during Initialize().
        public int CurrentUserId { get; private set; }

        public DaoContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            DbPath = Path.Combine(folder, "mauiapp4.db");
        }

        // Ensure DB and tables exist. Call once at startup.
        public void Initialize()
        {
            var builder = new SqliteConnectionStringBuilder { DataSource = DbPath };
            using var conn = new SqliteConnection(builder.ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            // Create Users table first
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Email TEXT,
                    CreatedAt TEXT
                );";
            cmd.ExecuteNonQuery();

            // Create Notes table with UserId relation
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Notes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT,
                    Content TEXT,
                    UserId INTEGER,
                    CreatedAt TEXT
                );";
            cmd.ExecuteNonQuery();

            // Ensure there's a current user: if none exists, insert a default one and store its Id
            cmd.CommandText = "SELECT Id FROM Users LIMIT 1;";
            var existing = cmd.ExecuteScalar();
            if (existing is null)
            {
                cmd.CommandText = "INSERT INTO Users (Name, Email, CreatedAt) VALUES ($name, $email, $createdAt);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("$name", "Jean Dupont");
                cmd.Parameters.AddWithValue("$email", "jean.dupont@example.com");
                cmd.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT last_insert_rowid();";
                var idObj = cmd.ExecuteScalar();
                CurrentUserId = idObj is null ? 0 : Convert.ToInt32(idObj);
            }
            else
            {
                CurrentUserId = Convert.ToInt32(existing);
            }
        }

        public SqliteConnection CreateConnection()
        {
            var builder = new SqliteConnectionStringBuilder { DataSource = DbPath };
            return new SqliteConnection(builder.ConnectionString);
        }
    }
}
