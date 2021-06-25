using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace file_converter
{

    public class file_xml : abstractFile
    {
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
        public string file_name { get; set; }
        public file_xml() { file_name = ""; }
        public file_xml(string s) { file_name = s; }
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

            d.fields = f.Distinct().ToList();
            d.values = v;
            return (d);
        }
        public void exporter(Data d)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(this.file_name);
            XmlElement xRoot = xDoc.DocumentElement;


            for (int i = 0; i < d.values.Count; i++)
            {
                XmlElement ObjectElem = xDoc.CreateElement("Object");
                for (int j = 0; j < d.fields.Count; j++)
                {
                    XmlElement PropertyElem = xDoc.CreateElement("Property");
                    XmlAttribute typeAttr = xDoc.CreateAttribute("type");
                    XmlAttribute nameAttr = xDoc.CreateAttribute("name");

                    XmlText PropertyText = xDoc.CreateTextNode(d.values[i][j]);
                    XmlText typeText = xDoc.CreateTextNode("string");
                    XmlText nameText = xDoc.CreateTextNode(d.fields[j]);

                    PropertyElem.AppendChild(PropertyText);
                    typeAttr.AppendChild(typeText);
                    nameAttr.AppendChild(nameText);

                    PropertyElem.Attributes.Append(typeAttr);
                    PropertyElem.Attributes.Append(nameAttr);
                    ObjectElem.AppendChild(PropertyElem);
                }
                xRoot.AppendChild(ObjectElem);
            }

            xDoc.Save(file_name);
        }
    }
}
