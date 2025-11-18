using System.Windows.Input;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Business;

namespace MauiApp4.ViewModels
{
    public class NoteViewModel : BaseViewModel
    {
        private readonly NoteService _noteService;
        public RoutingViewModel Router { get; }

        private int _id;
        public int Id { get => _id; set => SetProperty(ref _id, value); }

        private string _title = string.Empty;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private string _content = string.Empty;
        public string Content { get => _content; set => SetProperty(ref _content, value); }

        public ICommand SaveCommand { get; }

        public NoteViewModel(RoutingViewModel router, NoteService noteService)
        {
            Router = router;
            _noteService = noteService;

            SaveCommand = new Microsoft.Maui.Controls.Command(async () => await SaveAsync());
        }

        public async Task SaveAsync()
        {
            if (Id == 0)
            {
                var created = await _noteService.CreateNoteAsync(Title, Content);
                Id = created.Id;
            }
            else
            {
                var note = new Note { Id = Id, Title = Title, Content = Content };
                await _noteService.EditNoteAsync(note);
            }

            // Navigate back to Acceuil after save
            await Shell.Current.GoToAsync("///Acceuil");
        }
    }
}
 
