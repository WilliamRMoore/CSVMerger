using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public class FileStreamProvider : IFileStreamProvider
    {
        public FileStreamProvider()
        {

        }
        public StreamReader GetStream(string filePath)
        {
            return new StreamReader(filePath);
        }

        public bool IsFilePathValid(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
