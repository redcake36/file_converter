using System;
using System.Linq;

namespace file_converter
{   
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Must specify two files");
                Console.ReadLine();
            }
            else { 
                Data mydata = new Data();

                string[] frmt = { "txt", "csv", "json", "xml" };
                string[] arg_i = args[0].Split(".");
                string[] arg_o = args[1].Split(".");
                
                if (frmt.Contains(arg_i[arg_i.Length - 1]) & frmt.Contains(arg_o[arg_o.Length - 1]))
                {
                    abstractFile inFtype = new file_txt();
                    abstractFile outFtype = new file_txt();

                    //parse
                    switch (arg_i[arg_i.Length - 1])
                    {
                        case "txt":
                            inFtype = new file_txt(args[0]);
                            break;
                        case "csv":
                            inFtype = new file_csv(args[0]);
                            break;
                        case "json":
                            inFtype = new file_json(args[0]);
                            break;
                        case "xml":
                            inFtype = new file_xml(args[0]);
                            break;
                        default:
                            break;
                    }
                    try
                    {
                        mydata = inFtype.parser();
                    }
                    catch
                    {
                        Console.WriteLine("Can`t parse {0}", args[0]);
                    }
                    

                    //export
                    switch (arg_o[arg_o.Length - 1])
                    {
                        case "txt":
                            outFtype = new file_txt(args[1]);
                            break;
                        case "csv":
                            outFtype = new file_csv(args[1]);
                            break;
                        case "json":
                            outFtype = new file_json(args[1]);
                            break;
                        case "xml":
                            outFtype = new file_xml(args[1]);
                            break;
                        default:
                            break;
                    }
                    try
                    {
                        outFtype.exporter(mydata);
                    }
                    catch
                    {
                        if (arg_o[arg_o.Length - 1] == "xml")
                            Console.WriteLine("Can`t export...\nMaybe file {0} has no root \nOR \n{1} has bad structure", args[1], args[0]);
                        else
                            Console.WriteLine("Can`t export...\nMaybe file {0} has bad structure", args[0]);
                    }

                    Console.WriteLine("Done!");
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
}
