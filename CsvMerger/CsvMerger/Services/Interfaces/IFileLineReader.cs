using System;
using System.Collections.Generic;
using System.Text;
using static CsvMerger.Services.ServiceLayers.MapSetService;

namespace CsvMerger.Services.Interfaces
{
    public interface IFileLineReader
    {
        long CountLines(string filePath);
        List<string> LineReader(string filePath, string[] resultArray, List<int[]> mappingRules, CalcPercent calcPercent);
    }
}
