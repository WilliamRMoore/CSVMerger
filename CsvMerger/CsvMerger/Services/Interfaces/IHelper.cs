using CsvMerger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    public interface IHelper
    {
        bool ValidateSets(string filePath, string[] sets);
        bool DoesRuleExist(CsvSet set, int rule);
        CsvSet MapSets(List<CsvSet> dataSets, CsvSet ResultDataSet);
        List<CsvSet> LoadDataSets(List<CsvSet> dataSets);
        string FormatDataSetsList(IEnumerable<FileInfo> filenames);
        void MakeFile(CsvSet set);
    }
}
