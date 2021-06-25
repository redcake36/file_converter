using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace file_converter
{
    public class file_json : abstractFile
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
        public string file_name { get; set; }
        public file_json() { file_name = ""; }
        public file_json(string s) { file_name = s; }
        public Data parser()
        {
            Data d = new Data();
            string[] lines = System.IO.File.ReadAllLines(file_name);
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
            d.fields = f.Distinct().ToList();
            d.values = v;
            return (d);
        }
        public void exporter(Data d)
        {
            if (d.fields.Count == 0)
            {
                return;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(this.file_name))
                {
                    sw.WriteLine("[");
                    for (int i = 0; i < d.values.Count; i++)
                    {
                        sw.WriteLine("\t{");
                        for (int j = 0; j < d.fields.Count; j++)
                        {
                            sw.Write("\t\t\"{0}\": \"{1}\"", d.fields[j], d.values[i][j]);
                            if (j == d.fields.Count - 1)
                                sw.Write("\n");
                            else
                                sw.Write(",\n");
                        }

                        if (i == d.values.Count - 1)
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
