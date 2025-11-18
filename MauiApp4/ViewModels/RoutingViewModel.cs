using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp4.ViewModels
{
    // Centralized routing/navigation viewmodel following MVVM.
    public class RoutingViewModel
    {
        public Command NavigateToNoteCommand { get; }
        public Command NavigateToProfilCommand { get; }
        public Command NavigateToAcceuilCommand { get; }

        public RoutingViewModel()
        {
            NavigateToNoteCommand = new Command(async () => await Shell.Current.GoToAsync("///Note"));
            NavigateToProfilCommand = new Command(async () => await Shell.Current.GoToAsync("///Profil"));
            NavigateToAcceuilCommand = new Command(async () => await Shell.Current.GoToAsync("///Acceuil"));
        }
    }
}
