using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfNotebookProject.Classes
{
	public class TopMenuTabItems : PropertyChangedStuff
	{
		public List<TabItem> Items { get; set; }

		// Path of the file to auto-save when clickin save
		public List<string> FilePaths { get; set; }

		// The data/text from opened files
		public List<string> FileData { get; set; }

		private int _selectedItem;
		public int SelectedItem
		{
			get { return _selectedItem; }
			// nameof gets the name	of the variable and converts to a string
			set { _selectedItem = value; RaisePropertyChanged(nameof(SelectedItem)); }
		}

		public TopMenuTabItems()
		{
			Items = new List<TabItem>();
			FilePaths = new List<string>();
			FileData = new List<string>();
		}


		// VARIABLES
		private string _defaultName = "New Item";



		public void AddNewMenuTabItem()
		{
			
		}
	}
}
