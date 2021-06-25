using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System.Xml;

namespace file_converter
{
    public class Data
    {
        public List<string> fields;
        public List<List<string>> values;


        public void xml_parser(string file_name)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(file_name);
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
        }
    }
    
    class Program
    {
        public static Data csv_parser(string file_name)
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
        public static Data txt_parser(string file_name)
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
                    v.Add(a);
                }
                
            }
            d.fields = f.Distinct().ToList();
            d.values = v;
            return (d);
        }
        public static string jsonTakeWords(string[] lines,int i,ref int j)
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
                        s+=lines[i][j];
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
        public static string xmlTakeWords(string[] lines, int i, ref int j,string c)
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
        public static Data json_parser(string file_name)
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
        public static Data xml_parser(string file_name)
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
                        f.Add(xmlTakeWords(lines, i, ref j,"\"\""));
                        a.Add(xmlTakeWords(lines, i, ref j,"><"));

                        i++;
                    }
                    v.Add(a);
                }

            }

            d.fields = f.Distinct().ToList();
            d.values = v;
            return (d);
        }
        public static void txt_exporter(Data d, string file_name)
        {
            if (d.fields.Count == 0)
            {
                return;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(file_name))
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
        public static void csv_exporter(Data d, string file_name)
        {
            if (d.fields.Count == 0)
            {
                return;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(file_name))
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
        public static void xml_exporter(Data d, string file_name)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(file_name);
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
        public static void json_exporter(Data d, string file_name)
        {
            if (d.fields.Count == 0)
            {
                return;
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(file_name))
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
        static void Main(string[] args)
        {
            Data mydata = new Data();
            //mydata = csv_parser(args[1]);
            //txt_exporter(mydata,args[0]);
            string[] frmt = { "txt", "csv", "json", "xml" };
            string[] arg_z = args[0].Split(".");
            string[] arg_o = args[1].Split(".");
            if (frmt.Contains(arg_z[arg_z.Length - 1]) & frmt.Contains(arg_o[arg_o.Length - 1]))
            {
                abstractFile ftype = new file_txt(args[0]);
                //mydata = ftype.parser();
                mydata = xml_parser(args[3]);
                txt_exporter(mydata, args[0]);

                foreach (List<string> v in mydata.values)
                {
                    foreach (string s in v)
                    {
                        Console.WriteLine(s);
                    }
                }

                //mydata.txt_parser(args[0]);

                //for (int i = 0; i < mydata.fields.Count; i++)
                //{
                //    Console.WriteLine(mydata.fields[i]);
                //}



                //Console.WriteLine("Введите строку для записи в файл:");
                //string text = Console.ReadLine();
                //File.WriteAllText(args[0], text);



                //string[] lines = System.IO.File.ReadAllLines(args[1]);
                //foreach (string line in lines)
                //{
                //    Console.WriteLine(line);
                //}

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Can`t convert such type of file :C");
                Console.ReadLine();
            }

        }
    }
}
