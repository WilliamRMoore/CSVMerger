using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger
{
    class DataSet
    {
        public string[] Columns { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        //public string[] Rows { get; set; }
        public List<int[]> MapRules = new List<int[]>();

        public List<string> OutputRows = new List<string>();
    }
}
