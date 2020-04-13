using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
using CsvMerger.Services.ServiceLayers.TuiRoutines.InitialDataInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvMerger
{
    public class App
    {

        private readonly IServiceOrchestrator _ServiceOrchestrator;
        private readonly IInitialDataInput _initialDataInput;

        public App(IServiceOrchestrator orchestrator,IInitialDataInput initialDataInput)
        {
            _ServiceOrchestrator = orchestrator;
            _initialDataInput = initialDataInput;
        }

        public void Run()
        {
            CsvSet outputDataSet = new CsvSet();
            List<CsvSet> dataSets = new List<CsvSet>();
            var path = Directory.GetCurrentDirectory();
            var dataSetsPath = path + @"\DataSets";

            bool directoryExists = Directory.Exists(path + @"\DataSets");
            if (!directoryExists)
            {
                try
                {
                    Directory.CreateDirectory(path + "\\DataSets");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            _initialDataInput.Welcome();

            var newSetName = _initialDataInput.GetNewDatasetName();

            outputDataSet.FileName = newSetName;

            var columns = _initialDataInput.GetColumnNames();

            outputDataSet.Columns = columns;

            var outputDirectory = _initialDataInput.GetTargetDirectory();

            outputDataSet.OutpuFilePath = outputDirectory + "\\" + outputDataSet.FileName + ".csv";//check this

            _initialDataInput.AskUserIfDataIsCorrect(outputDataSet.FileName, string.Join(",", outputDataSet.Columns));

//--------------------------------------------------------------------------------------------------------------------------
            
            var dataSetsContents = new DirectoryInfo(dataSetsPath);
            Console.WriteLine("Current Contents of DataSets folder\n");
            var filenames = dataSetsContents.EnumerateFiles();

            var fileList = _ServiceOrchestrator.FormatDataSetsList(filenames);// FormatDataSetsList(filenames);

            Console.WriteLine(fileList);

            Console.WriteLine("If contents blank, please make sure to copy over the datasets you \n" +
                $"wish to merge into the DataSets folder located in {path + @"\DataSets"}.\n");

            Console.WriteLine("Please list the data sets you wish to merge seperated by commas.\n");


            do
            {
                var mergeSets = Console.ReadLine();

                var sets = mergeSets.Split(",");

                if (_ServiceOrchestrator.ValidateSets(dataSetsPath, sets))
                {
                    foreach (var s in sets)
                    {
                        CsvSet dataSet = new CsvSet();
                        dataSet.FileName = s;
                        dataSet.InputFilePath = dataSetsPath + $@"\{s}";

                        dataSets.Add(dataSet);
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("Please make sure the files you specify exist, and are spelled correctly.\n");
                    Console.WriteLine(fileList);
                    Console.WriteLine("Please list the data sets you wish to merge seperated by commas.\n");
                }

            } while (true);

            dataSets = _ServiceOrchestrator.LoadDataSetsColumns(dataSets);

//--------------------------------------------------------------------------------------------------------------------------
            
            foreach (var ds in dataSets)
            {
                Console.Clear();
                Console.WriteLine($"Please map which columns from dataset {ds.FileName} you want \n" +
                    $"to the correct column on {outputDataSet.FileName}.");
                foreach (var c in ds.Columns)
                {
                    foreach (var oc in outputDataSet.Columns)
                    {
                        if (_ServiceOrchestrator.DoesRuleExist(ds, Array.IndexOf(outputDataSet.Columns, oc)))
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine($"Map [{ds.FileName}] column \"{c}\" --> [{outputDataSet.FileName}] column \"{oc}\"?");
                            Console.WriteLine("\n");
                            Console.WriteLine("[Y,N]?");
                        }

                        var input = Console.ReadLine().ToUpper();
                        while (true)
                        {

                            if (input.Equals("Y"))
                            {
                                int[] rule = { Array.IndexOf(ds.Columns, c), Array.IndexOf(outputDataSet.Columns, oc) };
                                ds.MapRules.Add(rule);
                                break;
                            }
                            else if (input.Equals("N"))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input.");
                            }
                        }
                        if (input.Equals("Y"))
                        {
                            break;
                        }
                    }
                }
            }

//--------------------------------------------------------------------------------------------------------------------------
           
            outputDataSet = _ServiceOrchestrator.MapSets(dataSets, outputDataSet);

            Console.WriteLine("\nData is ready to be coppied into file format.");

            _ServiceOrchestrator.MakeFile(outputDataSet);

            Console.WriteLine("File Successfully created\n" +
                "\n" +
                "Press any key to terminate.");

            Console.ReadKey();

            System.Environment.Exit(1);
        }
    }
}
