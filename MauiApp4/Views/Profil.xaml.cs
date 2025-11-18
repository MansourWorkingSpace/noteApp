namespace MauiApp4.Views;

public partial class Profil : ContentPage
{
    public Profil()
    {
        InitializeComponent();
        var services = MauiApp4.App.Services ?? throw new System.InvalidOperationException("Services not configured");
        var noteService = services.GetService(typeof(Services.Business.NoteService)) as Services.Business.NoteService ?? throw new System.InvalidOperationException("NoteService missing");
        var userService = services.GetService(typeof(Services.Business.UserService)) as Services.Business.UserService ?? throw new System.InvalidOperationException("UserService missing");

        BindingContext = new ViewModels.ProfilViewModel(App.Router, userService, noteService);
    }
}