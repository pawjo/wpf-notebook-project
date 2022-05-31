﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            get => _openSections ?? (_openSections = LoadSections());
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
                OpenNote = OpenNotes.First();
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
            }
        }

        public string NoteText
        {
            get => OpenNote != null ? OpenNote.Text : GetNewNoteText();
            set
            {
                if (OpenNote != null)
                {
                    OpenNote.Text = value;
                }
            }
        }

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

        private RelayCommand _addNewNoteCommand;

        public RelayCommand AddNewNoteCommand
        {
            get
            {
                return _addNewNoteCommand ??
                    (_addNewNoteCommand = new RelayCommand(x => AddNewNote(x)));
            }
        }

        private System.Windows.Documents.FlowDocument _doc;

        public System.Windows.Documents.FlowDocument Doc
        {
            get => _doc ?? (_doc = new System.Windows.Documents.FlowDocument());
            set
            {
                _doc = value;
            }
        }


        public MainViewModel() : base()
        {
            Notebook = new Notebook();
            Notebook.Sections = new List<Section>();
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

        private void ChangeOpenNote(object param)
        {
            var note = param as Note;
            OpenNote = note ?? null;
            OnPropertyChanged(nameof(NoteText));
        }

        private ObservableCollection<Section> LoadSections()
        {
            if (Notebook.Sections == null)
            {
                Notebook.Sections = new List<Section>();
            }
            var sections = new ObservableCollection<Section>(Notebook.Sections);
            if (sections.Count == 0)
            {
                sections.Add(GetNewSection());
            }
            return sections;
        }

        private void AddNewTab()
        {
            var newSection = GetNewSection();
            Notebook.Sections.Add(newSection);
            OpenSections= LoadSections();
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
                ChangeOpenNote(newNote);
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


        private string GetNewNoteText() => XamlWriter.Save(new System.Windows.Documents.FlowDocument());

        private ObservableCollection<Note> GetNotesFromActualSection()
        {
            var notes = new ObservableCollection<Note>(ActualSection.Notes);
            OpenNote = notes.First();
            return notes;
        }
    }
}
