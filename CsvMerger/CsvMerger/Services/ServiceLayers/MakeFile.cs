using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public class MakeFile : IMakeFile
    {
        private readonly IPercentageCounter _percentageCounter;
        private readonly IFileStreamProvider _fileStream;
        private readonly IFileLineWriter _fileLineWriter;

        public MakeFile(IPercentageCounter percentageCounter, IFileStreamProvider fileStream, IFileLineWriter fileLineWriter)
        {
            _percentageCounter = percentageCounter;
            _fileStream = fileStream;
            _fileLineWriter = fileLineWriter;
        }

        public void MakeOutputFile(CsvSet set)
        {
            set.OutpuFilePath = set.InputFilePath + "\\" + set.FileName + ".csv"; //This should be done when rpgram receives input
            _percentageCounter.SetTotalItems(set.OutputRows.Count());
            var file = _fileStream.GetWriteStream(set.OutpuFilePath);
            _fileLineWriter.WriteLines(file, set);
        }
    }
}
