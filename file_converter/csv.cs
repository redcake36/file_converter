using System.IO;
using System.Collections.Generic;

namespace file_converter
{

    public class file_csv : abstractFile
    {
        public string file_name { get; set; }
        public file_csv() { file_name = ""; }
        public file_csv(string s) { file_name = s; }
        public Data parser()
        {
            Data d = new Data();
            List<string> f = new List<string>();
            List<List<string>> v = new List<List<string>>();

            string[] lines = System.IO.File.ReadAllLines(file_name);
            int i = 0;
            string word = "";
            while (i < lines[0].Length)
            {
                while (lines[0][i] != ',' & i < lines[0].Length - 1)
                {
                    word += lines[0][i];
                    i++;
                }
                if (i == lines[0].Length - 1)
                {
                    word += lines[0][i];
                }
                f.Add(word);
                word = "";
                i++;
            }


            for (int k = 1; k < lines.Length; k++)
            {
                List<string> a = new List<string>();
                int u = 0;
                i = 0;
                word = "";
                while (i < lines[k].Length)
                {
                    while (lines[k][i] != ',' & i < lines[k].Length - 1)
                    {
                        word += lines[k][i];
                        i++;
                    }
                    if (i == lines[k].Length - 1)
                    {
                        word += lines[k][i];
                    }
                    a.Add(word);
                    word = "";
                    u++;
                    i++;
                }
                v.Add(a);
            }
            d.fields = f;
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
                    for (int j = 0; j < d.fields.Count; j++)
                    {
                        if (j == d.fields.Count - 1)
                        {
                            sw.Write("{0}\n", d.fields[j]);
                        }
                        else
                        {
                            sw.Write("{0},", d.fields[j]);
                        }
                    }
                    for (int i = 0; i < d.values.Count; i++)
                    {
                        for (int j = 0; j < d.fields.Count; j++)
                        {
                            if (j == d.fields.Count - 1)
                            {
                                sw.Write("{0}\n", d.values[i][j]);
                            }
                            else
                            {
                                sw.Write("{0},", d.values[i][j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
