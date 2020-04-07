using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public interface IResultsAndTime
    {
        void StartTimer();
        void StopAndResetTimer();
       // TimeSpan GetJobTime();
        void PrintJobTime();
        TimeSpan GetTimeSpan();
    }
    public class ResultsAndTime : IResultsAndTime
    {
        private  readonly Stopwatch Timer = new Stopwatch();
        public void StartTimer()
        {
            Timer.Start();
        }

        public void StopAndResetTimer()
        {
            //Timer.Stop();
            Timer.Reset();
        }

        public void PrintJobTime()
        {
            Console.WriteLine(Timer.Elapsed);
        }

        public TimeSpan GetTimeSpan()
        {
            return Timer.Elapsed;
        }

    }
}
