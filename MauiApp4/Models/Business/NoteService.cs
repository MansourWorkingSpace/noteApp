using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Persistence;

namespace MauiApp4.Models.Business
{
    public class NoteService
    {
        private readonly INoteDao _noteDao;
        private readonly Infrastructure.Persistence.DaoContext _context;
        private readonly Services.Web.INotesApiService _notesApiService;

        public NoteService(INoteDao noteDao, Infrastructure.Persistence.DaoContext context, Services.Web.INotesApiService notesApiService)
        {
            _noteDao = noteDao;
            _context = context;
            _notesApiService = notesApiService;
        }

        // Return notes from the remote service (server-first), fall back to local DB
        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            try
            {
                var remote = await _notesApiService.GetNotesAsync();
                return remote ?? await _noteDao.GetAllAsync();
            }
            catch
            {
                return await _noteDao.GetAllAsync();
            }
        }

        // Create a note on the server first, then persist a local cache copy
        public async Task<Note> CreateNoteAsync(string title, string content)
        {
            var note = new Note { Title = title, Content = content, UserId = _context.CurrentUserId, CreatedAt = DateTime.UtcNow };
            try
            {
                var created = await _notesApiService.CreateNoteAsync(note);
                if (created is not null)
                {
                    // persist local copy of server-created note
                    var local = await _noteDao.AddAsync(created);
                    return local;
                }
            }
            catch
            {
                // ignore and fallback to local insert
            }

            // fallback: persist locally
            return await _noteDao.AddAsync(note);
        }

        // Edit an existing note on the server first, then update local cache
        public async Task<Note> EditNoteAsync(Note note)
        {
            try
            {
                var updated = await _notesApiService.UpdateNoteAsync(note.Id, note);
                if (updated is not null)
                {
                    await _noteDao.UpdateAsync(updated);
                    return updated;
                }
            }
            catch
            {
                // ignore and fallback to local update
            }

            return await _noteDao.UpdateAsync(note);
        }

        // Get notes for the current user (server-first)
        public async Task<IEnumerable<Note>> GetNotesForCurrentUserAsync()
        {
            try
            {
                var remote = await _notesApiService.GetNotesAsync();
                if (remote is null) return await _noteDao.GetUserNotesAsync(_context.CurrentUserId);
                return remote.Where(n => n.UserId == _context.CurrentUserId);
            }
            catch
            {
                return await _noteDao.GetUserNotesAsync(_context.CurrentUserId);
            }
        }

        // Get last N notes for the current user (server-first)
        public async Task<IEnumerable<Note>> GetLastNotesForCurrentUserAsync(int count = 3)
        {
            var notes = await GetNotesForCurrentUserAsync();
            return notes.OrderByDescending(n => n.CreatedAt).Take(count);
        }

        public async Task<Note?> GetNoteByIdAsync(int id)
        {
            try
            {
                var remote = await _notesApiService.GetNoteAsync(id);
                if (remote is not null) return remote;
            }
            catch
            {
                // ignore
            }
            return await _noteDao.GetByIdAsync(id);
        }

        // Expose direct remote fetch if needed
        public async Task<IEnumerable<Note>> GetNotesFromServerAsync()
        {
            var list = await _notesApiService.GetNotesAsync();
            return list ?? Enumerable.Empty<Note>();
        }
    }
}
