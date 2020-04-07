using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public interface IPercentageCounter
    {
        void SetTotalItems(decimal totalItems);
        void CalcPercent();
    }
    public class PercentageCounter : IPercentageCounter
    {
        private decimal TotalItems;
        private decimal processedItems;
        private decimal previousPercent;
        private readonly IResultsAndTime resultsAndTime;

        public PercentageCounter(/*Add ILogger here*/ IResultsAndTime resultsAndTime)
        {
            this.resultsAndTime = resultsAndTime;
        }

        public void SetTotalItems(decimal totalItems)
        {
            processedItems = 0;
            previousPercent = 0;
            TotalItems = totalItems;
        }

        public void CalcPercent()
        {
            processedItems++;
            var percentDone = Math.Floor((processedItems / TotalItems) * 100);

            if (processedItems == 1)
            {
                Console.WriteLine("0% Done");
                resultsAndTime.StartTimer();
            }

            if (previousPercent < percentDone)
            {
                Console.WriteLine($"{percentDone}% Done");
                previousPercent = percentDone;
            }

            if (percentDone == 100)
            {
                var time = resultsAndTime.GetTimeSpan();
                resultsAndTime.StopAndResetTimer();

                Console.WriteLine($"Job completed with {processedItems} records processed in {time.TotalSeconds} seconds.");
            }
        }
    }
}
