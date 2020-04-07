using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvMerger.Services.ServiceLayers
{

    public class MapSetService : IMapSetService
    {
        private readonly IPercentageCounter _percentageCounter;
        private readonly IJobCounter jobCounter;
        private readonly ITimeEstimator timeEstimator;
        private readonly IFileLineReader _fileLineReader;

        public MapSetService(IFileLineReader fileLineReader, IPercentageCounter percentageCounter,
            IJobCounter jobCounter, ITimeEstimator timeEstimator)
        {
            _fileLineReader = fileLineReader;
            _percentageCounter = percentageCounter;
            this.jobCounter = jobCounter;
            this.timeEstimator = timeEstimator;
        }

        public List<string> CsvSetLooper(List<CsvSet> csvSets, int ResultSetSize)
        {
            List<string> outputRows = new List<string>();
            
            foreach(var csv in csvSets)
            {
                string[] resultArray = new string[ResultSetSize];
                outputRows.AddRange(_fileLineReader.LineReader(csv.InputFilePath, resultArray, csv.MapRules));
            }

            return outputRows;
        }

        public CsvSet MapSets(List<CsvSet> csvSets, CsvSet resultCsvSet)
        {
            _percentageCounter.SetTotalItems(jobCounter.GetJobCount(csvSets));
            timeEstimator.EstimateTime(csvSets);
            resultCsvSet.OutputRows = CsvSetLooper(csvSets,resultCsvSet.Columns.Length);
            return resultCsvSet;
        }

    }

}
