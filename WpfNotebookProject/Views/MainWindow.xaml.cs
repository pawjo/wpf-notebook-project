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
            DataContext = _viewModel;
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
        }
    }
}
