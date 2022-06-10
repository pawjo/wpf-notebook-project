using System.Windows;
using WpfNotebookProject.ViewModels;

namespace WpfNotebookProject
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void NewNotebookButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMainWindow(true);
        }

        private void OpenNotebookButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMainWindow(false);
        }

        private void OpenMainWindow(bool isNewFile)
        {
            var viewModel = new MainViewModel(isNewFile);
            var window = new MainWindow(viewModel);
            this.Close();
            window.Show();
        }
    }
}
