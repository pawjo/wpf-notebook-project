using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNotebookProject.Applications
{
	public static class ThisApplication
	{
        public static ApplicationViewModel App { get; private set; }

        public static string UnclosedFilesStorageLocation { get; }

        public static string SharpPadFolderLocation { get; }

        //static ThisApplication()
        //{
        //    SharpPadFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SharpPadFiles");
        //    UnclosedFilesStorageLocation = Path.Combine(SharpPadFolderLocation, "UnclosedFiles");

        //    if (!Directory.Exists(UnclosedFilesStorageLocation))
        //        CreateUnclosedFilesStorageDirectory();
        //}
    }
}
