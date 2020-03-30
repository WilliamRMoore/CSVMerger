using CsvMerger.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    interface IHelper
    {
        bool ValidateSets(string filePath, string[] sets);
        bool DoesRuleExist(DataSet set, int rule);
        DataSet MapSets(List<DataSet> dataSets, DataSet ResultDataSet);
        List<DataSet> LoadDataSets(List<DataSet> dataSets);
        string FormatDataSetsList(IEnumerable<FileInfo> filenames);
        void MakeFile(DataSet set);
    }
}
