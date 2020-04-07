using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public interface IJobCounter
    {
        long GetJobCount(List<CsvSet> csvSets);
    }
    public class JobCounter : IJobCounter
    {
        private readonly IFileLineReader fileLineReader;

        public JobCounter(IFileLineReader fileLineReader)
        {
            this.fileLineReader = fileLineReader;
        }

        public long GetJobCount(List<CsvSet> csvSets)
        {
            //Gets the total row count for all Source csv's. 
            long rowCount = 0;

            foreach(var cs in csvSets)
            {
                rowCount += fileLineReader.CountLines(cs.InputFilePath);
            }

            return rowCount;
        }
    }
}
