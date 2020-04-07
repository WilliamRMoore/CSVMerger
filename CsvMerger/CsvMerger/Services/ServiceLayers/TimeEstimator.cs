using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvMerger.Services.ServiceLayers
{
    public interface ITimeEstimator
    {
        //int GetColumnAvrg(List<CsvSet> sets);
        void EstimateTime(List<CsvSet> sets);
        //List<string> SetUpTestStrings(int numOfColumns, int numOfTestRows);
    }

    public class TimeEstimator : ITimeEstimator
    {
        private readonly IRowProcessor rowProcessor;
        private readonly IResultsAndTime resultsAndTime;
        private readonly IJobCounter jobCounter;

        public TimeEstimator(IRowProcessor rowProcessor, IResultsAndTime resultsAndTime, IJobCounter jobCounter)
        {
            this.rowProcessor = rowProcessor;
            this.resultsAndTime = resultsAndTime;
            this.jobCounter = jobCounter;
        }
        public int GetColumnAvrg(List<CsvSet> sets)
        {
            var setCount = sets.Count();
            int totalNumberOfColumns = 0;

            foreach(var set in sets)
            {
                totalNumberOfColumns += set.Columns.Length;
            }

            var result = (double)(totalNumberOfColumns / setCount);

            result = Math.Round(result);

            return (int)result;
        }

        private List<int[]> MakeFakeRules(int resultSize, int attributeSize)
        {
            List<int[]> fakeRules = new List<int[]>();

            for(int i = resultSize; i > 0; i--)
            {
                Random random = new Random();

                int[] fakeRule = new int[2];

                int sourceRule;
                while (true)
                {
                    sourceRule = (int)Math.Round((random.NextDouble() * (attributeSize - 1)));

                    if(!DoesTestSourceRuleExist(fakeRules, sourceRule))
                    {
                        fakeRule[0] = sourceRule;
                        break;
                    }
                }

                int targetRule;

                while (true)
                {
                    targetRule = (int)Math.Round((random.NextDouble() * (resultSize - 1)));

                    if (!DoesTestTargetRuleExist(fakeRules, targetRule))
                    {
                        fakeRule[1] = targetRule;
                        break;
                    }
                }
                fakeRules.Add(fakeRule);
            }
            return fakeRules;
        }

        private bool DoesTestSourceRuleExist(List<int[]> rules, int rule)
        {
            return rules.Any(r => r[0] == rule);
        }

        private bool DoesTestTargetRuleExist(List<int[]> rules, int rule)
        {
            return rules.Any(r => r[1] == rule);
        }

        public void EstimateTime(List<CsvSet> sets)
        {
            //This entire function is terrible and needs to be replaced.
            var numOfTestRows = 50;
            var averageColumns = GetColumnAvrg(sets);
            var testStrings = SetUpTestStrings(averageColumns,numOfTestRows);
            List<string> testResult = new List<string>();
            var totalJobCount = jobCounter.GetJobCount(sets);
            string[] attributes;
            int resultSize = averageColumns / 2;
            string[] resultArray = new string[resultSize];
            var fakeRules = MakeFakeRules(resultSize, averageColumns);

            resultsAndTime.StartTimer();
            foreach(var test in testStrings)
            {
                attributes = rowProcessor.RowSplitter(test);
                resultArray = rowProcessor.RowMapper(fakeRules, attributes, resultArray);
                testResult.Add(string.Join(",", attributes));
            }

            var timeForTest = resultsAndTime.GetTimeSpan().TotalSeconds;
            resultsAndTime.StopAndResetTimer();

            var timeEstimateInSeconds = (double)((totalJobCount / numOfTestRows) * timeForTest);

            var timeEstimateInMinutes = (int)Math.Round((timeEstimateInSeconds / 60));

            Console.WriteLine($"Estimated time to completion is : {timeEstimateInMinutes} minutes.");
        }

        public List<string> SetUpTestStrings(int numOfColumns ,int numOfTestRows)
        {
            //creates a list of test strings

            List<string> testStrings = new List<string>();

            for(int i = numOfTestRows; i > 0; i--)
            {
                //outer loop creates each "row"

                var testString = "";

                for(int j = numOfColumns; j >0; j--)
                {
                    //inner loop creates a row that has the same number of attributes as "numOfColumns".

                    if(j > 1)//If j is not the last index, add a comma to the end of "TEST".
                    {
                        testString += "TEST,";
                    }
                    else//If j is the last index, don't add a comma to the end of "TEST"
                    {
                        testString += "TEST";
                    }
                }
                testStrings.Add(testString);
            }

            return testStrings;
        }
    }
}
