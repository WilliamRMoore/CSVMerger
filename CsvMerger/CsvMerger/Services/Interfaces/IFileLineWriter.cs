using CsvMerger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    public interface IFileLineWriter
    {
        void WriteLines(StreamWriter fileWriter, CsvSet outputSet);
    }
}
