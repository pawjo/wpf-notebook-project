using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfNotebookProject.Models;

namespace WpfNotebookProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Notebook Notebook { get; set; }

        private ObservableCollection<Section> _openSections;

        public ObservableCollection<Section> OpenSections
        {
            get
            {
                if (_openSections == null)
                {
                    _openSections = new ObservableCollection<Section>();
                }
                return _openSections;
            }
        }

        public Note OpenNote { get; set; }

        public string NoteText { get => OpenNote != null ? OpenNote.Text : string.Empty; }

        private RelayCommand _changeOpenNoteCommand;

        public RelayCommand ChangeOpenNoteCommand
        {
            get
            {
                return _changeOpenNoteCommand ??
                    (_changeOpenNoteCommand = new RelayCommand(x => ChangeOpenNote(x)));
            }
        }

        private RelayCommand _newTabCommand;

        public RelayCommand NewTabCommand
        {
            get
            {
                return _newTabCommand ??
                    (_newTabCommand = new RelayCommand(x => AddNewTab()));
            }
        }

        private void AddNewTab()
        {
            OpenSections.Add(new Section
            {
                Title = "Nowa sekcja",
                Notes = new List<Note>()
            });
        }


        public MainViewModel() : base()
        {
            Notebook = new Notebook();
            Notebook.Sections = new List<Section>
            {
                new Section
                {
                    Title = "Sekcja 1",
                    Notes = new List<Note>
                    {
                        new Note{Title="Testowa notatka 1", Text= "Tekst testowej notatki 1"},
                        new Note{Title="Test2", Text="Tekst 2"}
                    }
                },
                new Section
                {
                    Title="Sekcja 2",
                    Notes = new List<Note>
                    {
                        new Note{Title="Testowa notatka w sekcji 2", Text="aaaaaaa"},
                        new Note{Title="Test2", Text="Tekst 2"}
                    }
                }
            };
            LoadSections();
        }


        private void ChangeOpenNote(object param)
        {
            var note = param as Note;
            OpenNote = note ?? null;
            OnPropertyChanged(nameof(NoteText));
        }

        private void LoadSections()
        {
            if (Notebook != null)
            {
                _openSections = new ObservableCollection<Section>(Notebook.Sections);
            }
        }
    }
}
