using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Persistence;

namespace MauiApp4.Services.Business
{
    public class NoteService
    {
        private readonly INoteDao _noteDao;
        private readonly Infrastructure.Persistence.DaoContext _context;

        public NoteService(INoteDao noteDao, Infrastructure.Persistence.DaoContext context)
        {
            _noteDao = noteDao;
            _context = context;
        }

        public Task<IEnumerable<Note>> GetAllNotesAsync() => _noteDao.GetAllAsync();

        // Create a note for the current user
        public Task<Note> CreateNoteAsync(string title, string content)
        {
            var note = new Note { Title = title, Content = content, UserId = _context.CurrentUserId };
            return _noteDao.AddAsync(note);
        }

        // Edit an existing note
        public Task<Note> EditNoteAsync(Note note)
        {
            return _noteDao.UpdateAsync(note);
        }

        // Get notes for the current user
        public Task<IEnumerable<Note>> GetNotesForCurrentUserAsync()
            => _noteDao.GetUserNotesAsync(_context.CurrentUserId);

        // Get last N notes for the current user (default 3)
        public Task<IEnumerable<Note>> GetLastNotesForCurrentUserAsync(int count = 3)
            => _noteDao.GetLastNotesAsync(_context.CurrentUserId, count);
    }
}
