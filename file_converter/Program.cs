using System;
using System.Linq;

namespace file_converter
{
    class Program
    {
        private static string[] SUPPORTED_FORMATS = { "txt", "csv", "json", "xml" };
        private static string GetFileExtension(string filePath)
        {
            string[] s = filePath.Split("."); 
            return (s[s.Length-1]);
        }
        private static IFileWrapper BuildFileWrapper(string filePath)
        {
            string ext = GetFileExtension(filePath);
            switch (ext)
            {
                case "txt":
                    return new fileTxt(filePath);                   
                case "csv":
                    return new fileCsv(filePath);                   
                case "json":
                    return new fileJson(filePath);                   
                case "xml":
                    return new fileXml(filePath);
            }
            return new fileTxt(filePath);
        }
        static void Main(string[] args)
        {
            string firstArgExt = GetFileExtension(args[0]);
            string secondArgExt = GetFileExtension(args[1]);
            if (!SUPPORTED_FORMATS.Contains(firstArgExt) || !SUPPORTED_FORMATS.Contains(secondArgExt))
            {
                Console.WriteLine("Can`t convert such type of file :C");
                return;
            }
            if (args.Length != 2)
            {
                Console.WriteLine("Must specify two files");
                return;
            }
            else
            {
                Data myData = new Data();

                IFileWrapper inFtype = new fileTxt();
                IFileWrapper outFtype = new fileTxt();

                inFtype = BuildFileWrapper(args[0]);
                try
                {
                    myData = inFtype.Parse();
                }
                catch
                {
                    Console.WriteLine("Can`t parse {0}", args[0]);
                }

                outFtype = BuildFileWrapper(args[1]);
                try
                {
                    outFtype.Export(myData);
                }
                catch
                {
                    Console.WriteLine("Can`t export...\nMaybe file {0} has bad structure", args[0]);
                }

                Console.WriteLine("Done!");
            }
        }
    }
}
