using CsvMerger.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.Interfaces
{
    public interface IMakeFile
    {
        void MakeOutputFile(CsvSet set);
    }
}
