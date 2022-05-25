using System;
using System.Collections.Generic;

namespace WpfNotebookProject.Models
{
    public class Notebook
    {
        public string Name { get; set; }

        public string FileName { get; set; }

        public List<Section> Sections { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
