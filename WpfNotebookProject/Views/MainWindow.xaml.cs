using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfNotebookProject.Models;
using WpfNotebookProject.Themes;
using WpfNotebookProject.ViewModels;

namespace WpfNotebookProject
{
	public partial class MainWindow : Window
	{
		public App CurrentApplication { get; set; }

		private readonly MainViewModel _viewModel;

		public RichTextBox tbEditor
		{
			get
			{
				ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(SectionsTabControl);
				if (myContentPresenter != null)
				{
					DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
					// Finding textBlock from the DataTemplate that is set on that ContentPresenter
					return (RichTextBox)myDataTemplate.FindName("tbEditor", myContentPresenter);
				}
				return null;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new MainViewModel();
			DataContext = _viewModel;
			cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			cmbFontFamily.Text += Fonts.SystemFontFamilies.First().ToString();
			cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
			cmbFontSize.SelectedIndex = 4;
		}
		// zmiana
		private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog();
			dialog.ShowDialog();
		}
		// dark theme

		#region THEMES
		private void ChangeTheme(object sender, RoutedEventArgs e)
		{
			switch (int.Parse(((MenuItem)sender).Uid))
			{
				case 0:
					ThemesController.SetTheme(ThemesController.ThemeTypes.Light);
					break;
				case 1:
					ThemesController.SetTheme(ThemesController.ThemeTypes.ColourfulLight);
					break;
				case 2:
					ThemesController.SetTheme(ThemesController.ThemeTypes.Dark);
					break;
				case 3:
					ThemesController.SetTheme(ThemesController.ThemeTypes.ColourfulDark);
					break;
			}

			e.Handled = true;
		}


		private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cmbFontFamily.SelectedItem != null && tbEditor != null)
				tbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
		}


		private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!string.IsNullOrEmpty(cmbFontSize.Text) && tbEditor != null)
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

		private void testbtn(object sender, RoutedEventArgs e)
		{
			var blocks = tbEditor.Document.Blocks;
			var firstBlock = blocks.FirstBlock;
			;
		}
	}
}
