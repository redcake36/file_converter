using System;
using System.Linq;

namespace file_converter
{
    class Program
    {
        private static string[] SUPPORTED_FORMATS = { "txt", "csv", "json", "xml" };

        static void Main(string[] args)
        {
            if (!IsArgsCorr(args))
                return;

            IFileWrapper inFtype = BuildFileWrapper(args[0]);
            IFileWrapper outFtype = BuildFileWrapper(args[1]);

            Data myData = new Data();
            try {
                myData = inFtype.Parse();
                outFtype.Export(myData);
            }
            catch {
                Console.Error.WriteLine("Something went wrong ...");
            }

            Console.WriteLine("Done!");
        }
        private static bool IsArgsCorr(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Must specify two files");
                return false;
            }

            string firstArgExt = GetFileExtension(args[0]);
            string secondArgExt = GetFileExtension(args[1]);

            if (!SUPPORTED_FORMATS.Contains(firstArgExt) || !SUPPORTED_FORMATS.Contains(secondArgExt))
            {
                Console.WriteLine("Can`t convert such type of file");
                return false;
            }
            return true;
        }
        private static string GetFileExtension(string filePath)
        {
            string[] s = filePath.Split(".");
            return s[s.Length - 1];
        }
        private static IFileWrapper BuildFileWrapper(string filePath)
        {
            string ext = GetFileExtension(filePath);

            switch (ext)
            {
                case "csv":
                    return new FileCsv(filePath);
                case "json":
                    return new FileJson(filePath);
                case "xml":
                    return new FileXml(filePath);
                default:
                    return new FileTxt(filePath);
            }
        }

    }
}
