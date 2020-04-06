using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public interface IPercentageCounter
    {
        //decimal TotalItems { get; set; }
        void SetTotalItems(decimal totalItems);
        void CalcPercent();
    }
    public class PercentageCounter : IPercentageCounter
    {
        private decimal TotalItems;
        private decimal processedItems;
        private decimal previousPercent;
        public PercentageCounter(/*Add ILogger here*/)
        {
           
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
            //var previousPercent = Math.Floor(((processedItems - 1) / TotalItems) * 100);
            var percentDone = Math.Floor((processedItems / TotalItems) * 100);

            if(processedItems == 1)
            {
                Console.WriteLine("0% Done");
            }

            if(previousPercent < percentDone)
            {
                Console.WriteLine($"{percentDone}% Done");
                previousPercent = percentDone;
            }
        }
    }
}
