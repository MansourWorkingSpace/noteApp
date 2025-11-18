namespace MauiApp4.ViewModels
{
    public class ProfilViewModel
    {
        public RoutingViewModel Router { get; }

        // Services that the profile page may use
        private readonly Services.Business.UserService _userService;
        private readonly Services.Business.NoteService _noteService;

        public ProfilViewModel(RoutingViewModel router, Services.Business.UserService userService, Services.Business.NoteService noteService)
        {
            Router = router;
            _userService = userService;
            _noteService = noteService;
        }
    }
}
