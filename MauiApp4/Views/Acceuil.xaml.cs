namespace MauiApp4.Views;

public partial class Acceuil : ContentPage
{
    public Acceuil()
    {
        InitializeComponent();
        // Resolve services from DI and create the ViewModel
        var services = MauiApp4.App.Services ?? throw new System.InvalidOperationException("Services not configured");
        var noteService = services.GetService(typeof(Services.Business.NoteService)) as Services.Business.NoteService ?? throw new System.InvalidOperationException("NoteService missing");
        var userService = services.GetService(typeof(Services.Business.UserService)) as Services.Business.UserService ?? throw new System.InvalidOperationException("UserService missing");

        var vm = new ViewModels.AcceuilViewModel(App.Router, noteService, userService);
        BindingContext = vm;

        // Load notes immediately
        vm.LoadNotesCommand.Execute(null);
    }
}