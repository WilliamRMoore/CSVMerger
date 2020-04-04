using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvMerger.Services.ServiceLayers
{
    //public interface IFileStreamProvider
    //{
    //    StreamReader GetStream(string filePath);
    //    bool IsFilePathValid(string filePath);
    //}
    //public class FileStreamProvider : IFileStreamProvider
    //{
    //    public FileStreamProvider()
    //    {

    //    }
    //    public StreamReader GetStream(string filePath)
    //    {
    //        return new StreamReader(filePath);
    //    }

    //    public bool IsFilePathValid(string filePath)
    //    {
    //        return File.Exists(filePath);
    //    }
    //}
    public class MapSetService : IMapSetService
    {
        PercentageCounter percentageCounter = new PercentageCounter();

        private readonly IFileLineReader _fileLineReader;

        public MapSetService(IFileLineReader fileLineReader)
        {
            _fileLineReader = fileLineReader;
        }

        public delegate void CalcPercent();

        //public string[] RowSplitter(string row)
        //{
        //    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        //    var attributes = CSVParser.Split(row);
        //    return attributes;
        //}

        //public string[] RowMapper(List<int[]> rules, string[] attributes, string[] resultArray)
        //{
        //    foreach(var rule in rules)
        //    {
        //        resultArray[rule[1]] = attributes[rule[0]];
        //    }

        //    return resultArray;
        //}

        //public StreamReader GetFileStream(string filePath)
        //{
        //    StreamReader file = new StreamReader(filePath);
        //    return file;
        //}

        //public long CountLinesInFile(StreamReader file)
        //{
        //    long count = 0;
        //    file.ReadLine();

        //    while(file.EndOfStream != true)
        //    {
        //        count += 1;
        //        file.ReadLine();
        //    }

        //    return count;
        //}

        private long GetJobCount(List<CsvSet> dataSets)
        {
            long rowCount = 0;

            foreach (var ds in dataSets)
            {
                //rowCount += CountLinesInFile(GetFileStream(ds.FilePath));
                rowCount += _fileLineReader.CountLines(ds.InputFilePath);
            }

            return rowCount;
        }

        //public List<string> FileLineReader(string filePath, string[] resultArray, List<int[]> mappingRules)
        //{//Potentially problematic.
        //    List<string> fileRows = new List<string>();
        //    string[] attributes;
        //    var file = GetFileStream(filePath);
        //    var count = CountLinesInFile(file);
        //    file.DiscardBufferedData();
        //    file.BaseStream.Seek(0, SeekOrigin.Begin);
        //    file.ReadLine();

        //    for (long i = count; i > 0; i--)
        //    {
        //        //Business logic here
        //        attributes = RowSplitter(file.ReadLine());
        //        resultArray = RowMapper(mappingRules, attributes, resultArray);
        //        fileRows.Add(string.Join(",", resultArray));
        //        //End of business logic.
        //        percentageCounter.CalcPercent();
        //    }

        //    file.Close();
        //    return fileRows;
        //}

        public List<string> CsvSetLooper(List<CsvSet> csvSets, int ResultSetSize)
        {
            List<string> outputRows = new List<string>();
            CalcPercent calcPercent = percentageCounter.CalcPercent;

            foreach(var csv in csvSets)
            {
                string[] resultArray = new string[ResultSetSize];
                outputRows.AddRange(_fileLineReader.LineReader(csv.InputFilePath, resultArray, csv.MapRules, calcPercent));
            }

            return outputRows;
        }

        public CsvSet MapSets(List<CsvSet> csvSets, CsvSet resultCsvSet)
        {
            percentageCounter.TotalItems = GetJobCount(csvSets);
            resultCsvSet.OutputRows = CsvSetLooper(csvSets,resultCsvSet.Columns.Length);
            return resultCsvSet;
        }

    }

}
