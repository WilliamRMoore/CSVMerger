using CsvMerger.Services.ServiceLayers.TuiInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers.TuiRoutines
{
    public class UXTextOutput : IUXTextOutput
    {
        public UXTextOutput()
        {

        }

        public void OutputStringNewLine(string output)
        {
            Console.WriteLine(output);
        }

        public void OutputStringSameLine(string output)
        {
            Console.Write(output);
        }

        public void Beep()
        {
            Console.Beep();
        }
    }
}
