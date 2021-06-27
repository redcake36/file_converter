using System;
using System.Collections.Generic;
using System.Text;

namespace file_converter
{
    public interface IFileWrapper
    {
        string filePath { get; }
        Data Parse();
        void Export(Data input);
    }
}
