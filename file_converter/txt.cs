using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace file_converter
{
    public class file_txt : abstractFile
    {
        public string file_name { get; set; }
        public file_txt() { file_name = ""; }
        public file_txt(string s) { file_name = s; }
        public Data parser()
        {
            Data d = new Data();
            string[] lines = System.IO.File.ReadAllLines(file_name);
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
                    for (int i = 0; i < d.values.Count; i++)
                    {
                        sw.WriteLine("=== Object ===");
                        for (int j = 0; j < d.fields.Count; j++)
                        {
                            sw.WriteLine("{0}: {1}", d.fields[j], d.values[i][j]);
                        }
                        sw.WriteLine("=== End of object ===");
                    }
                }
            }
        }
    }
}
