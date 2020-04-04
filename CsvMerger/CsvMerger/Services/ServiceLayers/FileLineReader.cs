using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    //public interface IFileLineReader
    //{
    //    long CountLines(StreamReader fileStream);
    //}
    //public class FileLineReader : IFileLineReader
    //{
    //    private readonly IFileStreamProvider _fileStreamProvider;
    //    public FileLineReader(IFileStreamProvider fileStreamProvider)
    //    {
    //        _fileStreamProvider = fileStreamProvider;
    //    }
    //    public long CountLines(string filePath)
    //    {
    //        var reader = _fileStreamProvider.GetStream(filePath);
    //        long count = 0;
    //       reader.ReadLine();

    //        while (reader.EndOfStream != true)
    //        {
    //            count += 1;
    //            reader.ReadLine();
    //        }

    //        return count;
    //    }
    //}
}
