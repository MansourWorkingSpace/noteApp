namespace MauiApp4.Views;

public partial class Note : ContentPage
{
    public Note()
    {
        InitializeComponent();
        var services = MauiApp4.App.Services ?? throw new System.InvalidOperationException("Services not configured");
        var noteService = services.GetService(typeof(Services.Business.NoteService)) as Services.Business.NoteService ?? throw new System.InvalidOperationException("NoteService missing");

        BindingContext = new ViewModels.NoteViewModel(App.Router, noteService);
    }
}