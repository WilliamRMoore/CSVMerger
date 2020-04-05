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
        public StreamReader GetReadStream(string filePath)
        {
            return new StreamReader(filePath);
        }

        public StreamWriter GetWriteStream(string filePath)
        {
            return new StreamWriter(filePath);
        }

        public bool IsFilePathValid(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
