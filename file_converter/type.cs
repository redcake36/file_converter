using System;
using System.Collections.Generic;
using System.Text;

namespace file_converter
{
    public interface abstractFile
    {
        public string file_name { get; set; }
        public Data parser();
        public void exporter(Data d, string file_name);
    }
    public class file_txt : abstractFile
    {
        public string file_name { get; set; }
        public Data parser()
        {
            return new Data();
        }
        public void exporter(Data d, string file_name)
        {

        }
    }
    public class file_json : abstractFile
    {
        public string file_name { get; set; }
        public Data parser()
        {
            return new Data();
        }
        public void exporter(Data d, string file_name)
        {

        }
    }
    public class file_csv : abstractFile
    {
        public string file_name { get; set; }
        public Data parser()
        {
            return new Data();
        }
        public void exporter(Data d, string file_name)
        {

        }
    }
    public class file_xml : abstractFile
    {
        public string file_name { get; set; }
        public Data parser()
        {
            return new Data();
        }
        public void exporter(Data d, string file_name)
        {

        }
    }
}
