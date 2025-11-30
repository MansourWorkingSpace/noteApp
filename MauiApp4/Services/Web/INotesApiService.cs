using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;

namespace MauiApp4.Services.Web
{
    public interface INotesApiService
    {
        Task<List<Note>> GetNotesAsync();
        Task<Note?> GetNoteAsync(int id);
        Task<Note?> CreateNoteAsync(Note note);
        Task<Note?> UpdateNoteAsync(int id, Note note);
        Task<bool> DeleteNoteAsync(int id);
        Task<User?> GetUserAsync(int userId = 1);
    }
}
