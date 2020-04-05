using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvMerger.Services.ServiceLayers
{
    public class Helper : IHelper
    {
        private readonly IMapSetService _mapSetService;
        public Helper(IMapSetService mapSetService)
        {
            _mapSetService = mapSetService;
        }
        private decimal GetJobCount(List<CsvSet> dataSets)
        {
            decimal rowCount = 0.0m;

            foreach (var ds in dataSets)
            {
                using (StreamReader lineCounter = new StreamReader(ds.InputFilePath))
                {
                    while (lineCounter.ReadLine() != null)
                    {
                        rowCount += 1;
                    }
                }
                rowCount -= 1;
            }

            return rowCount;
        }

        private decimal PercentageCounter(decimal rowsProcessed, decimal rowCount, decimal previousPercent)
        {
            var percentDone = Math.Floor((rowsProcessed / rowCount) * 100);
            if (previousPercent < percentDone)
            {
                Console.WriteLine($"{percentDone}% Done");
            }

            return percentDone;
        }

        public  bool ValidateSets(string filePath, string[] sets)
        {
            List<string> pathsToMergeFiles = new List<string>();
            foreach (var s in sets)
            {
                var pathOfSet = filePath + $@"\{s}";
                if (!File.Exists(pathOfSet))
                {
                    return false;
                }
            }
            return true;
        }

        public bool DoesRuleExist(CsvSet set, int rule)
        {
            return set.MapRules.Any(r => r[1] == rule);
        }

        public CsvSet MapSets(List<CsvSet> dataSets, CsvSet ResultDataSet)
        {
            return _mapSetService.MapSets(dataSets, ResultDataSet);
        }

        public List<CsvSet> LoadDataSets(List<CsvSet> dataSets)
        {
            foreach (var ds in dataSets)
            {
                ds.Columns = File.ReadLines(ds.InputFilePath).First().Split(",");
            }

            return dataSets;
        }

        public string FormatDataSetsList(IEnumerable<FileInfo> filenames)
        {
            string outputFileList = "";
            string format = "{0,-15} {1,15}";


            for (int i = 0; i < filenames.Count(); i = i + 2)
            {
                var filename1 = filenames.ElementAt(i).Name;
                string filename2 = "";
                if (i != filenames.Count() - 1)
                {
                    filename2 = filenames.ElementAt(i + 1).Name;
                }

                outputFileList += string.Format(format, filename1, filename2);
                outputFileList += "\n";
            }

            return outputFileList;
        }

        public void MakeFile(CsvSet set)
        {
            decimal rowCount = set.OutputRows.Count();
            decimal rowProccessed = 0.0m;
            decimal percentDone = 0.0m;

            try
            {
                using (StreamWriter output = new StreamWriter(set.InputFilePath + "\\" + set.FileName + ".csv"))
                {
                    output.WriteLine(String.Join(",", set.Columns));

                    foreach (var row in set.OutputRows)
                    {
                        var previousPercent = percentDone;
                        output.WriteLine(row);
                        rowProccessed += 1;

                        percentDone = PercentageCounter(rowProccessed, rowCount, previousPercent);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
