using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using WpfNotebookProject.Models;
using WpfNotebookProject.Utils;

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

        private bool _isEditNoteTitleMode;

        public bool IsEditNoteTitleMode
        {
            get => _isEditNoteTitleMode;
            set
            {
                _isEditNoteTitleMode = value;
                OnPropertyChanged(nameof(IsEditNoteTitleMode));
                OnPropertyChanged(nameof(NoteTitleTextBlockVisibility));
                OnPropertyChanged(nameof(NoteTitleTextBoxVisibility));
            }
        }

        public Visibility NoteTitleTextBlockVisibility
        {
            get => _isEditNoteTitleMode ? Visibility.Collapsed : Visibility.Visible;
        }

        public Visibility NoteTitleTextBoxVisibility
        {
            get => _isEditNoteTitleMode ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool _isEditSectionTitleMode;

        public bool IsEditSectionTitleMode
        {
            get => _isEditSectionTitleMode;
            set
            {
                _isEditSectionTitleMode = value;
                OnPropertyChanged(nameof(IsEditSectionTitleMode));
                OnPropertyChanged(nameof(SectionTitleTextBlockVisibility));
                OnPropertyChanged(nameof(SectionTitleTextBoxVisibility));
            }
        }

        public Visibility SectionTitleTextBlockVisibility
        {
            get => _isEditSectionTitleMode ? Visibility.Collapsed : Visibility.Visible;
        }

        public Visibility SectionTitleTextBoxVisibility
        {
            get => _isEditSectionTitleMode ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool IsSaved { get; set; }

        public string FilePath { get; set; }

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


        private RelayCommand _enableEditNoteTitleCommand;

        public RelayCommand EnableEditNoteTitleCommand
        {
            get => _enableEditNoteTitleCommand ??
                (_enableEditNoteTitleCommand = new RelayCommand(x =>
                {
                    IsEditNoteTitleMode = true;
                }));
        }

        private RelayCommand _disableEditNoteTitleCommand;

        public RelayCommand DisableEditNoteTitleCommand
        {
            get => _disableEditNoteTitleCommand ??
                (_disableEditNoteTitleCommand = new RelayCommand(x =>
                {
                    IsEditNoteTitleMode = false;
                }));
        }

        private RelayCommand _selectAllTextCommand;

        public RelayCommand SelectAllTextCommand
        {
            get => _selectAllTextCommand ??
                (_selectAllTextCommand = new RelayCommand(x =>
                {
                    var textBox = x as TextBox;
                    textBox.SelectAll();
                }));
        }

        private RelayCommand _deleteNoteCommand;

        public RelayCommand DeleteNoteCommand
        {
            get => _deleteNoteCommand ??
                (_deleteNoteCommand = new RelayCommand(x =>
                {
                    var note = x as Note;
                    if (note != null)
                    {
                        ActualSection.Notes.Remove(note);
                        OpenNotes = GetNotesFromActualSection();
                        OpenNote = OpenNotes.First();
                    }
                }));
        }

        private RelayCommand _enableEditSectionTitleCommand;

        public RelayCommand EnableEditSectionTitleCommand
        {
            get => _enableEditSectionTitleCommand ??
                (_enableEditSectionTitleCommand = new RelayCommand(x =>
                {
                    IsEditSectionTitleMode = true;
                }));
        }

        private RelayCommand _disableEditSectionTitleCommand;

        public RelayCommand DisableEditSectionTitleCommand
        {
            get => _disableEditSectionTitleCommand ??
                (_disableEditSectionTitleCommand = new RelayCommand(x =>
                {
                    IsEditSectionTitleMode = false;
                }));
        }

        private RelayCommand _deleteSectionCommand;

        public RelayCommand DeleteSectionCommand
        {
            get => _deleteSectionCommand ??
                (_deleteSectionCommand = new RelayCommand(x =>
                {
                    if (OpenSections.Count > 1)
                    {
                        Notebook.Sections.Remove(ActualSection);
                        ActualSection = OpenSections.First(x => x != ActualSection);
                        OpenSections = GetSections();
                        OpenNotes = GetNotesFromActualSection();
                        OpenNote = OpenNotes.First();
                    }
                }));
        }

        private RelayCommand _saveNotebookCommand;

        public RelayCommand SaveNotebookCommand
        {
            get => _saveNotebookCommand ??
                (_saveNotebookCommand = new RelayCommand(x =>
                {
                    if (!IsSaved && !GetFileNameForSave())
                    {
                        return;
                    }

                    SaveFile();
                }));
        }



        private RelayCommand _saveNotebookAsCommand;

        public RelayCommand SaveNotebookAsCommand
        {
            get => _saveNotebookAsCommand ??
                (_saveNotebookAsCommand = new RelayCommand(x =>
                {
                    if (!GetFileNameForSave())
                    {
                        return;
                    }

                    SaveFile();
                }));
        }

        

        private RelayCommand _openNotebookCommand;

        public RelayCommand OpenNotebookCommand
        {
            get => _openNotebookCommand ??
                (_openNotebookCommand = new RelayCommand(x =>
                {
                    OpenNotebook();
                }));
        }

        

        private RelayCommand _newNotebookCommand;

        public RelayCommand NewNotebookCommand
        {
            get => _newNotebookCommand ??
                (_newNotebookCommand = new RelayCommand(x =>
                {
                    OpenEmptyNotebook();
                }));
        }

        private RelayCommand _closeNotebookCommand;

        public RelayCommand CloseNotebookCommand
        {
            get => _closeNotebookCommand ??
                (_closeNotebookCommand = new RelayCommand(x =>
                {
                    if (!IsSaved)
                    {
                        var result = MessageBox.Show("Czy chcesz zapisać zmiany?", "Zamykanie notesu", MessageBoxButton.YesNoCancel);
                        if(result==MessageBoxResult.Cancel)
                        {
                            return;
                        }
                        else if(result == MessageBoxResult.Yes && string.IsNullOrWhiteSpace(FilePath) && GetFileNameForSave())
                        {
                            SaveFile();
                        }
                        else if (result == MessageBoxResult.Yes)
                        {
                            SaveFile();
                        }

                    }

                    OpenEmptyNotebook();
                }));
        }

        public MainViewModel(bool isNewFile) : base()
        {
            if (isNewFile)
            {
                OpenEmptyNotebook();
            }
            else
            {
                OpenNotebook();
            }
        }

        public void OpenNotebook()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Otwórz notes";
            dialog.Filter = "Pliki XML (*.xml)|*.xml|Wszystkie pliki (*.*)|*.*";
            if (dialog.ShowDialog() == false)
            {
                return;
            }
            var path = dialog.FileName;

            var notebook = XMLUtility.ReadNotebookFromFile(path);
            if (notebook == null)
            {
                MessageBox.Show("Błąd odczytu");
            }

            IsSaved = true;
            FilePath = path;
            Notebook = notebook;
            InitNotebook();
        }

        private void OpenEmptyNotebook()
        {
            Notebook = new Notebook();
            Notebook.Sections = new List<Section>();
            AddNewTab();
        }

        public void InitNotebook()
        {
            OpenSections = GetSections();
            ActualSection = OpenSections.First();
            OpenNotes = GetNotesFromActualSection();
            OpenNote = OpenNotes.First();
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

        private void SaveFile()
        {
            if (!XMLUtility.WriteNotebookToFile(Notebook, FilePath))
            {
                MessageBox.Show("Błąd zapisu");
            }
            IsSaved = true;
        }

        private bool GetFileNameForSave()
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "xml";
            dialog.Title = "Zapisz notes";
            if (dialog.ShowDialog() == false)
            {
                return false;
            }
            FilePath = dialog.FileName;
            return true;
        }
    }
}
