using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Persistence;
using Microsoft.Data.Sqlite;

namespace MauiApp4.Infrastructure.Persistence
{
    public class NoteDao : INoteDao
    {
        private readonly DaoContext _context;

        public NoteDao(DaoContext context)
        {
            _context = context;
        }

        public async Task<Note> AddAsync(Note entity)
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Notes (Title, Content, UserId, CreatedAt) VALUES ($title, $content, $userId, $createdAt);";
            cmd.Parameters.AddWithValue("$title", entity.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("$content", entity.Content ?? string.Empty);
            cmd.Parameters.AddWithValue("$userId", entity.UserId);
            cmd.Parameters.AddWithValue("$createdAt", entity.CreatedAt.ToString("o"));

            await cmd.ExecuteNonQueryAsync();

            // get last inserted id
            cmd.CommandText = "SELECT last_insert_rowid();";
            var scalar = await cmd.ExecuteScalarAsync();
            var id = scalar is null ? 0L : Convert.ToInt64(scalar);
            entity.Id = (int)id;

            return entity;
        }

        // Deletion is intentionally not supported per requirements.

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            var list = new List<Note>();
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Content, UserId, CreatedAt FROM Notes ORDER BY CreatedAt DESC;";
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Note
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Content = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    UserId = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(4) ? null : rdr.GetString(4), out var dt) ? dt : DateTime.UtcNow
                });
            }
            return list;
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Content, UserId, CreatedAt FROM Notes WHERE Id = $id LIMIT 1;";
            cmd.Parameters.AddWithValue("$id", id);
            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new Note
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Content = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    UserId = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(4) ? null : rdr.GetString(4), out var dt) ? dt : DateTime.UtcNow
                };
            }
            return null;
        }

        public async Task<IEnumerable<Note>> SearchByTitleAsync(string query)
        {
            var list = new List<Note>();
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Content, UserId, CreatedAt FROM Notes WHERE Title LIKE $q;";
            cmd.Parameters.AddWithValue("$q", $"%{query}%");
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Note
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Content = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    UserId = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(4) ? null : rdr.GetString(4), out var dt) ? dt : DateTime.UtcNow
                });
            }
            return list;
        }

        public async Task<Note> UpdateAsync(Note entity)
        {
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Notes SET Title = $title, Content = $content WHERE Id = $id;";
            cmd.Parameters.AddWithValue("$title", entity.Title ?? string.Empty);
            cmd.Parameters.AddWithValue("$content", entity.Content ?? string.Empty);
            cmd.Parameters.AddWithValue("$id", entity.Id);
            await cmd.ExecuteNonQueryAsync();
            return entity;
        }

        public async Task<IEnumerable<Note>> GetUserNotesAsync(int userId)
        {
            var list = new List<Note>();
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Content, UserId, CreatedAt FROM Notes WHERE UserId = $uid ORDER BY CreatedAt DESC;";
            cmd.Parameters.AddWithValue("$uid", userId);
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Note
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Content = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    UserId = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(4) ? null : rdr.GetString(4), out var dt) ? dt : DateTime.UtcNow
                });
            }
            return list;
        }

        public async Task<IEnumerable<Note>> GetLastNotesAsync(int userId, int count)
        {
            var list = new List<Note>();
            using var conn = _context.CreateConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Title, Content, UserId, CreatedAt FROM Notes WHERE UserId = $uid ORDER BY CreatedAt DESC LIMIT $cnt;";
            cmd.Parameters.AddWithValue("$uid", userId);
            cmd.Parameters.AddWithValue("$cnt", count);
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new Note
                {
                    Id = rdr.GetInt32(0),
                    Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                    Content = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    UserId = rdr.IsDBNull(3) ? 0 : rdr.GetInt32(3),
                    CreatedAt = DateTime.TryParse(rdr.IsDBNull(4) ? null : rdr.GetString(4), out var dt) ? dt : DateTime.UtcNow
                });
            }
            return list;
        }
    }
}
