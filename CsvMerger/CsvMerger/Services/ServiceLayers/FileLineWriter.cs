using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public class FileLineWriter : IFileLineWriter
    {
        private readonly IPercentageCounter _percentageCounter;
        private readonly IFileStreamProvider _fileStream;

        public FileLineWriter(IPercentageCounter percentageCounter, IFileStreamProvider fileStream)
        {
            _percentageCounter = percentageCounter;
            _fileStream = fileStream;
        }

        public void WriteLines(string outputFilePath, CsvSet outputSet)
        {
            //Writes each line from the output csv set to the target location (outputFilePath). 
            var fileWriter = _fileStream.GetWriteStream(outputFilePath);

            using (fileWriter)
            {
                //One-liner for writing the column headers at the top of the file.
                fileWriter.WriteLine(String.Join(",", outputSet.Columns));

                foreach (var row in outputSet.OutputRows)
                {
                    fileWriter.WriteLine(row);
                    _percentageCounter.CalcPercent();
                }
            }
        }
    }
}
