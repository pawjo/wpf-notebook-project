using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WpfNotebookProject.Models;
using WpfNotebookProject.ViewModels;
using static WpfNotebookProject.Themes.ThemesController;

namespace WpfNotebookProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

		public RichTextBox tbEditor
		{
			get
			{
				ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(SectionsTabControl);

				// Finding textBlock from the DataTemplate that is set on that ContentPresenter
				DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
				return (RichTextBox)myDataTemplate.FindName("tbEditor", myContentPresenter);
			}
		}

		public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
			cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
		}
        // zmiana
        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
        }


        #region THEMES
        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 0: SetTheme(ThemeTypes.Light); break;
                case 1: SetTheme(ThemeTypes.ColourfulLight); break;
                case 2: SetTheme(ThemeTypes.Dark); break;
                case 3: SetTheme(ThemeTypes.ColourfulDark); break;
            }
        }

		private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbFontFamily.SelectedItem != null)
				tbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
		}

		private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
		{
			tbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
		}

		private void tbEditor_SelectionChanged(object sender, RoutedEventArgs e)
		{
			object temp = tbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
			btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
			temp = tbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
			btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
			temp = tbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
			btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

			temp = tbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
			cmbFontFamily.SelectedItem = temp;
			temp = tbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
			cmbFontSize.Text = temp.ToString();
		}



		#endregion
		//private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		//{
		//    if (PreferencesG.SAVE_OPEN_UNCLOSED_FILES)
		//    {
		//        ThisApplication.SaveAllUnclosedFilesToStorageLocation(this);
		//    }

		//    if (CanSavePreferences)
		//    {
		//        try
		//        {
		//            PreferencesG.CLOSE_NOTEPADLIST_BY_DEFAULT = !Notepad.LeftTabsExpanded;
		//            Properties.Settings.Default.closeTopNLstOnStrt = !Notepad.TopTabsExpanded;
		//            PreferencesG.SavePropertiesToFile();
		//            if (WindowState == WindowState.Maximized)
		//            {
		//                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
		//                Properties.Settings.Default.Height = RestoreBounds.Height;
		//                Properties.Settings.Default.Width = RestoreBounds.Width;
		//            }
		//            else
		//            {
		//                Properties.Settings.Default.Height = this.Height;
		//                Properties.Settings.Default.Width = this.Width;
		//            }
		//            switch (CurrentTheme)
		//            {
		//                case ThemeTypes.Light: Properties.Settings.Default.Theme = 1; break;
		//                case ThemeTypes.Dark: Properties.Settings.Default.Theme = 2; break;
		//                case ThemeTypes.ColourfulLight: Properties.Settings.Default.Theme = 3; break;
		//                case ThemeTypes.ColourfulDark: Properties.Settings.Default.Theme = 4; break;
		//            }
		//            if (!this.Notepad.IsNotepadDocumentsNull())
		//            {
		//                if (Notepad.Notepad.DocumentFormat != null)
		//                {
		//                    Properties.Settings.Default.DefaultFont = this.Notepad.Notepad.DocumentFormat.Family.ToString();
		//                    Properties.Settings.Default.DefaultFontSize = this.Notepad.Notepad.DocumentFormat.Size;
		//                }
		//            }
		//            Properties.Settings.Default.allowCaretLineOutline = showLineThing.IsChecked ?? true;
		//            Properties.Settings.Default.Save();
		//        }
		//        catch { }
		//        //Notepad?.ShutdownInformationHook();
		//    }

		//    WindowClosedCallback?.Invoke(this);
		//}



		private childItem FindVisualChild<childItem>(DependencyObject obj)
			where childItem : DependencyObject
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(obj, i);
				if (child != null && child is childItem)
				{
					return (childItem)child;
				}
				else
				{
					childItem childOfChild = FindVisualChild<childItem>(child);
					if (childOfChild != null)
						return childOfChild;
				}
			}
			return null;
		}

	}
}
