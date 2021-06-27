using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace file_converter
{
    public class fileJson : IFileWrapper
    {
        static string jsonTakeWords(string[] lines, int i, ref int j)
        {
            string s = "";
            bool flag = true;
            while (flag)
            {
                if (lines[i][j] == '"')
                {
                    j++;
                    while (lines[i][j] != '"')
                    {
                        s += lines[i][j];
                        j++;
                    }
                    j++;
                    flag = false;
                }
                else
                    j++;
            }
            return s;
        }
        public string filePath { get;}
        public fileJson() { filePath = ""; }
        public fileJson(string s) { filePath = s; }
        public Data Parse()
        {
            Data d = new Data();
            string[] lines = System.IO.File.ReadAllLines(filePath);
            List<string> f = new List<string>();
            List<List<string>> v = new List<List<string>>();

            for (int i = 0; i < lines.Length; i++)
            {
                List<string> a = new List<string>();
                if (lines[i].Contains('{'))
                {
                    i++;
                    while (!lines[i].Contains('}'))
                    {
                        int j = 0;
                        f.Add(jsonTakeWords(lines, i, ref j));
                        a.Add(jsonTakeWords(lines, i, ref j));

                        i++;
                    }
                    v.Add(a);
                }
            }
            d.FieldNames = f.Distinct().ToList();
            d.Content = v;
            return (d);
        }
        public void Export(Data input)
        {
            if (input.FieldNames.Count == 0)
            {
                return;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(this.filePath))
                {
                    sw.WriteLine("[");
                    for (int i = 0; i < input.Content.Count; i++)
                    {
                        sw.WriteLine("\t{");
                        for (int j = 0; j < input.FieldNames.Count; j++)
                        {
                            sw.Write("\t\t\"{0}\": \"{1}\"", input.FieldNames[j], input.Content[i][j]);
                            if (j == input.FieldNames.Count - 1)
                                sw.Write("\n");
                            else
                                sw.Write(",\n");
                        }

                        if (i == input.Content.Count - 1)
                            sw.WriteLine("\t}");
                        else
                            sw.WriteLine("\t},");

                    }
                    sw.Write("]");
                }

            }
        }
    }
}
