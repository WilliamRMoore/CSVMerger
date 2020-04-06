using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers.TuiInterfaces
{
    public interface IUXTextOutput
    {
        void OutputStringNewLine(string output);
        void OutputStringSameLine(string output);
        void Beep();
    }
}
