using MauiApp4.Models.Business;

namespace MauiApp4.Views;

public partial class Acceuil : ContentPage
{
    public Acceuil()
    {
        InitializeComponent();
        var services = MauiApp4.App.Services ?? throw new System.InvalidOperationException("Services not configured");
        var noteService = services.GetService(typeof(NoteService)) as NoteService ?? throw new System.InvalidOperationException("NoteService missing");
        var userService = services.GetService(typeof(UserService)) as UserService ?? throw new System.InvalidOperationException("UserService missing");

        var vm = new ViewModels.AcceuilViewModel(App.Router, noteService, userService);
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewModels.AcceuilViewModel vm)
        {
            // Refresh notes every time the page appears
            vm.LoadNotesCommand.Execute(null);
        }
    }
}