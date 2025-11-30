using MauiApp4.Models.Business;

namespace MauiApp4.Views;

public partial class Profil : ContentPage
{
    public Profil()
    {
        InitializeComponent();
        var services = MauiApp4.App.Services ?? throw new System.InvalidOperationException("Services not configured");
        var noteService = services.GetService(typeof(NoteService)) as NoteService ?? throw new System.InvalidOperationException("NoteService missing");
        var userService = services.GetService(typeof(UserService)) as UserService ?? throw new System.InvalidOperationException("UserService missing");
        var daoContext = services.GetService(typeof(Infrastructure.Persistence.DaoContext)) as Infrastructure.Persistence.DaoContext ?? throw new System.InvalidOperationException("DaoContext missing");

        var vm = new ViewModels.ProfilViewModel(App.Router, userService, noteService, daoContext);
        BindingContext = vm;
    }
}