using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp4.ViewModels
{
    // Centralized routing/navigation viewmodel following MVVM.
    public class RoutingViewModel
    {
        public Command<object> NavigateToNoteCommand { get; }
        public Command NavigateToProfilCommand { get; }
        public Command NavigateToAcceuilCommand { get; }

        public RoutingViewModel()
        {
            // Accept optional note id parameter; navigate to Note or Note with query param
            NavigateToNoteCommand = new Command<object>((param) =>
            {
                var route = param is null ? "///Note" : $"///Note?noteId={param}";
                _ = Shell.Current.GoToAsync(route);
            });
            NavigateToProfilCommand = new Command(async () => await Shell.Current.GoToAsync("///Profil"));
            NavigateToAcceuilCommand = new Command(async () => await Shell.Current.GoToAsync("///Acceuil"));
        }
    }
}
