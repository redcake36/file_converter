using System.IO;
using System.Collections.Generic;

namespace file_converter
{
    public class FileCsv : IFileWrapper
    {
        public string FilePath { get; }
        public FileCsv() { FilePath = ""; }
        public FileCsv(string s) { FilePath = s; }
        private string GetWord(string[] lines,ref int i, int k)
        {
            string word = "";
            while (lines[k][i] != ',' & i < lines[k].Length - 1)
            {
                word += lines[k][i];
                i++;
            }
            if (i == lines[k].Length - 1)
            {
                word += lines[k][i];
            }
            return word;
        }
        private void AddFileWord(StreamWriter sw,int n, List<string> l)
        {
            for (int j = 0; j < n; j++)
            {
                if (j == n - 1)
                    sw.Write("{0}\n", l[j]);
                else
                    sw.Write("{0},", l[j]);
            }
        }
        public Data Parse()
        {
            List<string> f = new List<string>();
            List<List<string>> v = new List<List<string>>();

            string[] lines = System.IO.File.ReadAllLines(FilePath);
            int i = 0;
            while (i < lines[0].Length)
            {
                f.Add(GetWord(lines,ref i,0));
                i++;
            }

            for (int k = 1; k < lines.Length; k++)
            {
                List<string> a = new List<string>();
                int u = 0;
                i = 0;
                while (i < lines[k].Length)
                {
                    a.Add(GetWord(lines, ref i, k));
                    u++;
                    i++;
                }
                v.Add(a);
            }
            return (new Data(f, v));
        }
        public void Export(Data input)
        {
            if (input.FieldNames.Count == 0)
                return;

            using (StreamWriter sw = new StreamWriter(this.FilePath))
            {
                AddFileWord(sw, input.FieldNames.Count, input.FieldNames);
                for (int i = 0; i < input.Content.Count; i++)
                {
                    AddFileWord(sw, input.FieldNames.Count, input.Content[i]);
                }
            }

        }
    }
}
