using System.IO;
using System.Xml.Serialization;
using WpfNotebookProject.Models;

namespace WpfNotebookProject.Utils
{
    public static class XMLUtility
    {
        public static bool WriteNotebookToFile(Notebook notebook, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var serializer = new XmlSerializer(typeof(Notebook));
            var file = File.Create(path);
            bool result;
            try
            {
                serializer.Serialize(file, notebook);
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                file.Close();
            }
            return result;
        }

        public static Notebook ReadNotebookFromFile(string path)
        {
            var serializer = new XmlSerializer(typeof(Notebook));
            var file = new StreamReader(path);
            Notebook result;
            try
            {
                result = (Notebook)serializer.Deserialize(file);
            }
            catch
            {
                result = null;
            }
            finally
            {
                file.Close();
            }
            return result;
        }
    }
}
