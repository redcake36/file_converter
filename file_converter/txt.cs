using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace file_converter
{
    public class FileTxt : IFileWrapper
    {
        public string FilePath { get; }
        public FileTxt() { FilePath = ""; }
        public FileTxt(string s) { FilePath = s; }
        public Data Parse()
        {
            string[] lines = System.IO.File.ReadAllLines(FilePath);
            List<string> f = new List<string>();
            List<List<string>> v = new List<List<string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                List<string> a = new List<string>();
                if (lines[i].Contains("Object"))
                {
                    i++;
                    while (!lines[i].Contains("End of object"))
                    {
                        string[] subs = lines[i].Split(": ");
                        f.Add(subs[0]);
                        a.Add(subs[1]);
                        i++;
                    }
                }
                v.Add(a);
            }
            return (new Data(f.Distinct().ToList(), v));
        }
        public void Export(Data input)
        {
            if (input.FieldNames.Count == 0)
                return;

            using (StreamWriter sw = new StreamWriter(this.FilePath))
            {
                for (int i = 0; i < input.Content.Count; i++)
                {
                    sw.WriteLine("=== Object ===");
                    for (int j = 0; j < input.FieldNames.Count; j++)
                    {
                        sw.WriteLine("{0}: {1}", input.FieldNames[j], input.Content[i][j]);
                    }
                    sw.WriteLine("=== End of object ===");
                }
            }
        }
    }
}
