using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public interface IPercentageCounter
    {
        void CalcPercent();
    }
    public class PercentageCounter : IPercentageCounter
    {
        public decimal TotalItems { get; set; }
        private decimal processedItems;
        public PercentageCounter(/*Add ILogger here*/)
        {
            Console.WriteLine("0% Done");
        }

        public void CalcPercent()
        {
            processedItems++;
            var previousPercent = Math.Floor(((processedItems - 1) / TotalItems) * 100);
            var percentDone = Math.Floor((processedItems / TotalItems) * 100);

            if(previousPercent < percentDone)
            {
                Console.WriteLine($"{percentDone}% Done");
            }
        }
    }
}
