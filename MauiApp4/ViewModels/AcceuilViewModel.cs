using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;
using MauiApp4.Models.Entities;
using MauiApp4.Models.Business;

namespace MauiApp4.ViewModels
{
    public class AcceuilViewModel : BaseViewModel
    {
        private readonly NoteService _noteService;
        private readonly UserService _userService;

        public RoutingViewModel Router { get; }

        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();

        private int _notesCount;
        public int NotesCount { get => _notesCount; set => SetProperty(ref _notesCount, value); }

        public ICommand LoadNotesCommand { get; }
        public ICommand SyncCommand { get; }

        public AcceuilViewModel(RoutingViewModel router, NoteService noteService, UserService userService)
        {
            Router = router;
            _noteService = noteService;
            _userService = userService;

            LoadNotesCommand = new Microsoft.Maui.Controls.Command(async () => await LoadAsync());
            SyncCommand = new Microsoft.Maui.Controls.Command(async () => await SyncAsync());
        }

        public async Task LoadAsync()
        {
            Notes.Clear();
            var notes = await _noteService.GetNotesForCurrentUserAsync();
            foreach (var n in notes)
                Notes.Add(n);

            NotesCount = Notes.Count;
        }

        private async Task SyncAsync()
        {
            // Fetch remote notes (demo) and append a few for demonstration (does not persist)
            var remote = await _noteService.GetNotesFromServerAsync();
            if (remote is null) return;

            // Add first 3 remote notes that are not already present (based on Id)
            var toAdd = remote.Take(3).Where(r => !Notes.Any(n => n.Id == r.Id));
            foreach (var r in toAdd)
                Notes.Insert(0, r);

            NotesCount = Notes.Count;
        }

        public IEnumerable<Note> LastThree => Notes.Take(3);
    }
}
