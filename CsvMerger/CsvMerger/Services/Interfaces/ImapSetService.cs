using CsvMerger.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    public interface IMapSetService
    {
        //string[] MapRow(int[] rule, string[] attributes, string[] resultArray);
        string[] RowSplitter(string row);
        //string[] RowMapper(List<int[]> rules, string[] attributes, string[] resultArray);
        CsvSet MapSets(List<CsvSet> csvSets, CsvSet resultCsvSet);
    }
}
