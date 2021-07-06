using System;
using System.Collections.Generic;
using System.Text;

namespace file_converter
{
    public class Data
    {
        public List<string> FieldNames;
        public List<List<string>> Content;
        public Data()
        {
            FieldNames = new List<string>();
            Content = new List<List<string>>();
        }

        public Data(List<string> f, List<List<string>> c)
        {
            FieldNames = f;
            Content = c;
        }
    }
}
