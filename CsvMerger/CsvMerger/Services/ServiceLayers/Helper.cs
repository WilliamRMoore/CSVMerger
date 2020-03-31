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
    class Helper : IHelper
    {
        private decimal GetJobCount(List<DataSet> dataSets)
        {
            decimal rowCount = 0.0m;

            foreach (var ds in dataSets)
            {
                using (StreamReader lineCounter = new StreamReader(ds.FilePath))
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

        public bool DoesRuleExist(DataSet set, int rule)
        {
            return set.MapRules.Any(r => r[1] == rule);
        }

        public DataSet MapSets(List<DataSet> dataSets, DataSet ResultDataSet)
        {
            decimal rowCount = GetJobCount(dataSets);
            decimal rowsProccessed = 0.0m;
            decimal percentDone = 0.0m;

            Console.WriteLine($"{0.00}% Done");


            foreach (var ds in dataSets)
            {
                string row;
                StreamReader file = new StreamReader(ds.FilePath);
                file.ReadLine();

                while ((row = file.ReadLine()) != null)
                {
                    string[] rowArray = new string[ResultDataSet.Columns.Length];
                    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    var attributes = CSVParser.Split(row);

                    foreach (var r in ds.MapRules)
                    {
                        rowArray[r[1]] = attributes[r[0]];
                    }

                    ResultDataSet.OutputRows.Add(string.Join(",", rowArray));
                    rowsProccessed += 1;
                    var previousPercent = percentDone;
                    percentDone = PercentageCounter(rowsProccessed, rowCount, previousPercent);

                }
                file.Close();
            }

            return ResultDataSet;
        }


        public List<DataSet> LoadDataSets(List<DataSet> dataSets)
        {
            foreach (var ds in dataSets)
            {
                ds.Columns = File.ReadLines(ds.FilePath).First().Split(",");
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

        public void MakeFile(DataSet set)
        {
            decimal rowCount = set.OutputRows.Count();
            decimal rowProccessed = 0.0m;
            decimal percentDone = 0.0m;

            try
            {
                using (StreamWriter output = new StreamWriter(set.FilePath + "\\" + set.FileName + ".csv"))
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
