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
            //Takes a row (string) that is parsed from the csv file, then uses a Regex to properly seperate the
            //values. Regex accounts for escaped characters and formatting.
            //returns an array of the attributes from the row.

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var attributes = CSVParser.Split(row);

            return attributes;
        }

        public string[] RowMapper(List<int[]> rules, string[] attributes, string[] resultArray)
        {
            //Maps source columns to target columns using rules (int arrays).
            //rule[0] is the source column, while rule [1] is the target column.
            //The int[] should probably (definitley) be replaced with an object.

            foreach (var rule in rules)
            {
                resultArray[rule[1]] = attributes[rule[0]];
            }

            return resultArray;
        }
    }
}
