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

        public FileLineWriter(IPercentageCounter percentageCounter)
        {
            _percentageCounter = percentageCounter;
        }

        public void WriteLines(StreamWriter fileWriter, CsvSet outputSet)
        {
            using (fileWriter)
            {
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
