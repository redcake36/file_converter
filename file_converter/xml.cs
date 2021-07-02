using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace file_converter
{
    public class FileXml : IFileWrapper
    {
        public string FilePath { get;}
        public FileXml() { FilePath = ""; }
        public FileXml(string s) { FilePath = s; }
        static string xmlTakeWords(string[] lines, int i, ref int j, string c)
        {
            string s = "";
            bool flag = true;
            if (j == 0)
            {
                j = lines[i].IndexOf("name");
            }

            while (flag)
            {
                if (lines[i][j] == c[0])
                {
                    j++;
                    while (lines[i][j] != c[1])
                    {
                        s += lines[i][j];
                        j++;
                    }
                    flag = false;
                }
                else
                    j++;
            }
            return s;
        }
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
                    while (!lines[i].Contains("/Object"))
                    {
                        int j = 0;
                        f.Add(xmlTakeWords(lines, i, ref j, "\"\""));
                        a.Add(xmlTakeWords(lines, i, ref j, "><"));
                        i++;
                    }
                    v.Add(a);
                }
            }
            return (new Data(f.Distinct().ToList(),v));
        }
        public void Export(Data input)
        {
            if (input.FieldNames.Count == 0)
                return;

            XDocument xDoc = new XDocument();
            XElement root = new XElement("List");

            for (int i = 0; i < input.Content.Count; i++)
            {
                XElement ObjectElem = new XElement("Object");
                for (int j = 0; j < input.FieldNames.Count; j++)
                {
                    XElement PropertyElem = new XElement("Property", input.Content[i][j]);
                    XAttribute typeAttr = new XAttribute("type","string");
                    XAttribute nameAttr = new XAttribute("name", input.FieldNames[j]);

                    PropertyElem.Add(typeAttr);
                    PropertyElem.Add(nameAttr);
                    ObjectElem.Add(PropertyElem);
                }
                root.Add(ObjectElem);
            }
            xDoc.Add(root);
            xDoc.Save(FilePath);
        }
    }
}
