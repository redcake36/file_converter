using System.Collections.Generic;

namespace file_converter
{
    public class Data
    {
        public List<string> fields;
        public List<List<string>> values;
    }
    public interface abstractFile
    {
        public string file_name { get; set; }
        public Data parser();
        public void exporter(Data d);
    }
    
}
