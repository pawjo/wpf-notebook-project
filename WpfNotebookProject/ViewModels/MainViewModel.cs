using WpfNotebookProject.Models;

namespace WpfNotebookProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Notebook Notebook { get; set; }

        public Note OpenNote { get; set; }

        public string NoteText { get => OpenNote != null ? OpenNote.Text : string.Empty; }

        public RelayCommand ChangeOpenNoteCommand
        {
            get
            {
                return _changeOpenNoteCommand ??
                    (_changeOpenNoteCommand = new RelayCommand(x => ChangeOpenNote(x)));
            }
        }

        private RelayCommand _changeOpenNoteCommand;

        public MainViewModel()
        {
        }


        private void ChangeOpenNote(object param)
        {
            var note = param as Note;
            OpenNote = note ?? null;
            OnPropertyChanged(nameof(NoteText));
        }
    }
}
