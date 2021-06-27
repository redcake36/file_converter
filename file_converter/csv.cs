using System.IO;
using System.Collections.Generic;

namespace file_converter
{

    public class fileCsv : IFileWrapper
    {
        public string filePath { get;}
        public fileCsv() { filePath = ""; }
        public fileCsv(string s) { filePath = s; }
        public Data Parse()
        {
            Data d = new Data();
            List<string> f = new List<string>();
            List<List<string>> v = new List<List<string>>();

            string[] lines = System.IO.File.ReadAllLines(filePath);
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
            d.FieldNames = f;
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
                    for (int j = 0; j < input.FieldNames.Count; j++)
                    {
                        if (j == input.FieldNames.Count - 1)
                        {
                            sw.Write("{0}\n", input.FieldNames[j]);
                        }
                        else
                        {
                            sw.Write("{0},", input.FieldNames[j]);
                        }
                    }
                    for (int i = 0; i < input.Content.Count; i++)
                    {
                        for (int j = 0; j < input.FieldNames.Count; j++)
                        {
                            if (j == input.FieldNames.Count - 1)
                            {
                                sw.Write("{0}\n", input.Content[i][j]);
                            }
                            else
                            {
                                sw.Write("{0},", input.Content[i][j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
