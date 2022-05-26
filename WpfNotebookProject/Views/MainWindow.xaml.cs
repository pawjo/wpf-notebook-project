using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using WpfNotebookProject.Models;
using WpfNotebookProject.ViewModels;

namespace WpfNotebookProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            _viewModel.Notebook = new Notebook();
            _viewModel.Notebook.Sections = new List<Section>
            {
                new Section
                {
                    Title = "Sekcja 1",
                    Notes = new List<Note>
                    {
                        new Note{Title="Testowa notatka 1"},
                        new Note{Title="Test2"}
                    }
                },
                new Section
                {
                    Title="Sekcja 2"
                }
            };

            DataContext = _viewModel;
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
        }
    }
}
