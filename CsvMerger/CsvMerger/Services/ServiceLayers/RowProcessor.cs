using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvMerger.Services.ServiceLayers
{
    public class RowProcessor : IRowProcessor
    {
        public string[] RowSplitter(string row)
        {
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var attributes = CSVParser.Split(row);

            return attributes;
        }

        public string[] RowMapper(List<int[]> rules, string[] attributes, string[] resultArray)
        {
            foreach (var rule in rules)
            {
                resultArray[rule[1]] = attributes[rule[0]];
            }

            return resultArray;
        }
    }
}
