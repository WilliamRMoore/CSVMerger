using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    public interface IFileStreamProvider
    {
        StreamReader GetStream(string filePath);
        bool IsFilePathValid(string filePath);
    }
}
