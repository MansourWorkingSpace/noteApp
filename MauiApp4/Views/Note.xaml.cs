using MauiApp4.Models.Business;
using Microsoft.Maui.Controls;

namespace MauiApp4.Views;

[QueryProperty(nameof(NoteId), "noteId")]
public partial class Note : ContentPage
{
    public string? NoteId
    {
        set
        {
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out var id) && BindingContext is ViewModels.NoteViewModel vm)
            {
                _ = vm.LoadAsync(id);
            }
        }
    }

    public Note()
    {
        InitializeComponent();
        var services = MauiApp4.App.Services ?? throw new System.InvalidOperationException("Services not configured");
        var noteService = services.GetService(typeof(NoteService)) as NoteService ?? throw new System.InvalidOperationException("NoteService missing");
        BindingContext = new ViewModels.NoteViewModel(App.Router, noteService);
    }
}