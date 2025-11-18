using MauiApp4.Models.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MauiApp4.Services.Persistence
{
    // Persistence contract for Note entities (SQLite implementation provided in Infrastructure)
    public interface INoteDao
    {
        Task<IEnumerable<Note>> GetAllAsync();
        Task<Note> AddAsync(Note entity);
        Task<Note?> GetByIdAsync(int id);
        // Update an existing note
        Task<Note> UpdateAsync(Note entity);
        // Get notes for a specific user
        Task<IEnumerable<Note>> GetUserNotesAsync(int userId);
        // Get last N notes for a specific user
        Task<IEnumerable<Note>> GetLastNotesAsync(int userId, int count);
        Task<IEnumerable<Note>> SearchByTitleAsync(string query);
    }
}
