using MauiApp4.Models.Business;

namespace MauiApp4.ViewModels
{
    public class ProfilViewModel : BaseViewModel
    {
        public RoutingViewModel Router { get; }

        // Services that the profile page may use
        private readonly UserService _userService;
        private readonly NoteService _noteService;

        private string _userName = "Utilisateur";
        public string UserName { get => _userName; set => SetProperty(ref _userName, value); }

        private int _notesCount;
        public int NotesCount { get => _notesCount; set => SetProperty(ref _notesCount, value); }

        public ProfilViewModel(RoutingViewModel router, UserService userService, NoteService noteService, Infrastructure.Persistence.DaoContext daoContext)
        {
            Router = router;
            _userService = userService;
            _noteService = noteService;

            // Load profile for the current user immediately; this keeps page code-behind free of initialization logic.
            _ = LoadAsync(daoContext.CurrentUserId);
        }

        // Load profile data (user name and note count) for the current user id
        public async System.Threading.Tasks.Task LoadAsync(int currentUserId)
        {
            var user = await _userService.GetByIdAsync(currentUserId);
            UserName = user?.Name ?? "Utilisateur";

            var notes = await _noteService.GetNotesForCurrentUserAsync();
            NotesCount = notes is null ? 0 : System.Linq.Enumerable.Count(notes);
        }
    }
}
