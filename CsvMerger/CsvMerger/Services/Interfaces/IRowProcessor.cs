using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    public interface IRowProcessor
    {
        string[] RowSplitter(string row);
        string[] RowMapper(List<int[]> rules, string[] attributes, string[] resultArray);
    }
}
