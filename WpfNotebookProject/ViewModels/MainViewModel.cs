using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WpfNotebookProject.Models;

namespace WpfNotebookProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public Notebook Notebook { get; set; }

        private ObservableCollection<Section> _openSections;

        public ObservableCollection<Section> OpenSections
        {
            get => _openSections ?? (_openSections = GetSections());
            set
            {
                _openSections = value;
                OnPropertyChanged(nameof(OpenSections));
            }
        }

        private Section _actualSection;

        public Section ActualSection
        {
            get => _actualSection ?? (_actualSection = OpenSections.First());
            set
            {
                _actualSection = value;
                OnPropertyChanged(nameof(ActualSection));
                OpenNotes = GetNotesFromActualSection();
            }
        }

        private ObservableCollection<Note> _openNotes;

        public ObservableCollection<Note> OpenNotes
        {
            get => _openNotes ?? (_openNotes = GetNotesFromActualSection());
            set
            {
                _openNotes = value;
                OnPropertyChanged(nameof(OpenNotes));
            }
        }

        private Note _openNote;
        public Note OpenNote
        {
            get => _openNote ?? (_openNote = OpenNotes.First());
            set
            {
                _openNote = value;
                OnPropertyChanged(nameof(OpenNote));
                OnPropertyChanged(nameof(NoteText));
            }
        }

        public string NoteText
        {
            get => OpenNote.Text;
            set
            {
                OpenNote.Text = value;
                OnPropertyChanged(nameof(NoteText));
            }
        }

        private bool _isEditMode;

        public Visibility TextBlockVisibility
        {
            get => _isEditMode ? Visibility.Collapsed : Visibility.Visible;
        }

        public Visibility TextBoxVisibility
        {
            get => _isEditMode ? Visibility.Visible : Visibility.Collapsed;
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

        private RelayCommand _addNewNoteCommand;

        public RelayCommand AddNewNoteCommand
        {
            get
            {
                return _addNewNoteCommand ??
                    (_addNewNoteCommand = new RelayCommand(x => AddNewNote(x)));
            }
        }

        private RelayCommand _changeModeCommand;

        public RelayCommand ChangeModeCommand
        {
            get => _changeModeCommand ?? (_changeModeCommand = new RelayCommand(x =>
            {
                _isEditMode = !_isEditMode;
                OnPropertyChanged(nameof(TextBlockVisibility));
                OnPropertyChanged(nameof(TextBoxVisibility));
            }));
        }

        //private RelayCommand _turnOnEditNoteTitleCommand;

        //public RelayCommand TurnOnEditNoteTitleCommand
        //{
        //    get=>_turnOnEditNoteTitleCommand ??
        //        (_turnOnEditNoteTitleCommand = new RelayCommand(x =>
        //        {
        //            var selectedItem = x as ListBoxItem;

        //            if (selectedItem != null)
        //            {
        //                var textBlock = selectedItem.
        //            }
        //        }
        //}

        public MainViewModel() : base()
        {
            Notebook = new Notebook();
            Notebook.Sections = new List<Section>();
            AddNewTab();
            //Notebook.Sections = new List<Section>
            //{
            //    new Section
            //    {
            //        Title = "Sekcja 1",
            //        Notes = new List<Note>
            //        {
            //            new Note{Title="Testowa notatka 1", Text= "Tekst testowej notatki 1"},
            //            new Note{Title="Test2", Text="Tekst 2"}
            //        }
            //    },
            //    new Section
            //    {
            //        Title="Sekcja 2",
            //        Notes = new List<Note>
            //        {
            //            new Note{Title="Testowa notatka w sekcji 2", Text="aaaaaaa"},
            //            new Note{Title="Test2", Text="Tekst 2"}
            //        }
            //    }
            //};
            //ChangeSection(Notebook.Sections[0]);
        }

        private void AddNewTab()
        {
            var newSection = GetNewSection();
            Notebook.Sections.Add(newSection);
            OpenSections = GetSections();
            ActualSection = newSection;
        }

        private void AddNewNote(object param)
        {
            var section = param as Section;
            if (section != null)
            {
                var newNote = GetNewNote();
                ActualSection.Notes.Add(newNote);
                OpenNotes = GetNotesFromActualSection();
                OpenNote = newNote;
            }
        }

        private Note GetNewNote() =>
            new Note
            {
                Title = "Nowa notatka",
                Text = GetNewNoteText()
            };

        private Section GetNewSection() =>
            new Section
            {
                Title = "Nowa sekcja",
                Notes = new List<Note> { GetNewNote() }
            };

        private string GetNewNoteText() =>
            XamlWriter.Save(new System.Windows.Documents.FlowDocument());

        private ObservableCollection<Section> GetSections() =>
            new ObservableCollection<Section>(Notebook.Sections);

        private ObservableCollection<Note> GetNotesFromActualSection() =>
            new ObservableCollection<Note>(ActualSection.Notes);
    }
}
