namespace WpfNotebookProject.Models
{
    public class Note
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public bool IdBold { get; set; }

        public bool IsUnderlined { get; set; }

        public Font Font { get; set; }

        public int FontSize { get; set; }
    }
}
