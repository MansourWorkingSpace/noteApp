using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Persistence;
using Microsoft.Data.Sqlite;

namespace MauiApp4.Infrastructure.Persistence
{
    public class UserDao : IUserDao
    {
        private readonly DaoContext _context;

        public UserDao(DaoContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User entity)
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Name, Email, CreatedAt) VALUES ($name, $email, $createdAt);";
            cmd.Parameters.AddWithValue("$name", entity.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("$email", entity.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("$createdAt", entity.CreatedAt.ToString("o"));
            await cmd.ExecuteNonQueryAsync();

            cmd.CommandText = "SELECT last_insert_rowid();";
            var scalar = await cmd.ExecuteScalarAsync();
            var id = scalar is null ? 0L : Convert.ToInt64(scalar);
            entity.Id = (int)id;
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Users WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var list = new List<User>();
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, Email, CreatedAt FROM Users ORDER BY CreatedAt DESC;";
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new User
                {
                    Id = rdr.GetInt32(0),
                    Name = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Email = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(3) ? null : rdr.GetString(3), out var dt) ? dt : DateTime.UtcNow
                });
            }
            return list;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, Email, CreatedAt FROM Users WHERE Id = $id LIMIT 1;";
            cmd.Parameters.AddWithValue("$id", id);
            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new User
                {
                    Id = rdr.GetInt32(0),
                    Name = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Email = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(3) ? null : rdr.GetString(3), out var dt) ? dt : DateTime.UtcNow
                };
            }
            return null;
        }

        public async Task<IEnumerable<User>> SearchByNameAsync(string query)
        {
            var list = new List<User>();
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Name, Email, CreatedAt FROM Users WHERE Name LIKE $q;";
            cmd.Parameters.AddWithValue("$q", $"%{query}%");
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new User
                {
                    Id = rdr.GetInt32(0),
                    Name = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Email = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(3) ? null : rdr.GetString(3), out var dt) ? dt : DateTime.UtcNow
                });
            }
            return list;
        }
    }
}
