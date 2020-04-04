using CsvMerger.Data;
using CsvMerger.Services.Interfaces;
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

        private readonly IHelper _helper;
        public App(IHelper helper)
        {
            _helper = helper;
        }

        public void Run()
        {
            CsvSet outputDataSet = new CsvSet();
            List<CsvSet> dataSets = new List<CsvSet>();
            var path = Directory.GetCurrentDirectory();
            var dataSetsPath = path + @"\DataSets";
            string newDataSetName = "";

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

            Console.WriteLine("Welcome to the .csv generator tool, this tool is used to merge disparate data sets \n" +
                "into a .arff file for use in the Weka data analysis program.");

            Console.WriteLine("\n Please specify the name of the dataset you would like to create \n" +
                "***WARNING DO NOT INCLUDE FILE EXTENSION IN NAME***\n" +
                "***WARNING DO NOT INCLUDE THE FOLLOWING CHARACTERS IN THE FILE NAME***\n" +
                "* . \" / \\ [ ] : ; | ,\n");

            Console.Write("New DataSet Name --> ");

            Regex regex1 = new Regex(@"^[a-zA-Z0-9_]+$");
            while (true)
            {
                newDataSetName = Console.ReadLine();

                if (!regex1.IsMatch(newDataSetName))
                {
                    Console.WriteLine("Please make sure your dataset name does not contain illegal characters.\n");
                    Console.Write("New DataSet Name -->");
                }
                else
                {
                    CsvSet dataSet = new CsvSet();
                    dataSet.FileName = newDataSetName;

                    Console.WriteLine("\nPlease List the Column Names seperated by commas.");
                    var columnNames = Console.ReadLine();
                    dataSet.Columns = columnNames.Split(",");
                    Console.WriteLine($"\nA data set with the name of {dataSet.FileName} containing the columns {columnNames} \n" +
                        $"will be generated. Do you wish to continue?");
                    Console.WriteLine("\n[Y/N]?");
                    while (true)
                    {
                        var input = Console.ReadLine().ToUpper();
                        if (input.Equals("Y"))
                        {
                            Console.Clear();
                            outputDataSet = dataSet;
                            break;
                        }
                        else if (input.Equals("N"))
                        {
                            System.Environment.Exit(1);
                        }
                    }
                    break;
                }
            }

            var dataSetsContents = new DirectoryInfo(dataSetsPath);
            Console.WriteLine("Current Contents of DataSets folder\n");
            var filenames = dataSetsContents.EnumerateFiles();

            var fileList = _helper.FormatDataSetsList(filenames);// FormatDataSetsList(filenames);

            Console.WriteLine(fileList);

            Console.WriteLine("If contents blank, please make sure to copy over the datasets you \n" +
                $"wish to merge into the DataSets folder located in {path + @"\DataSets"}.\n");

            Console.WriteLine("Please list the data sets you wish to merge seperated by commas.\n");


            do
            {
                var mergeSets = Console.ReadLine();

                var sets = mergeSets.Split(",");

                if (_helper.ValidateSets(dataSetsPath, sets))
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

            dataSets = _helper.LoadDataSets(dataSets);

            foreach (var ds in dataSets)
            {
                Console.Clear();
                Console.WriteLine($"Please map which columns from dataset {ds.FileName} you want \n" +
                    $"to the correct column on {outputDataSet.FileName}.");
                foreach (var c in ds.Columns)
                {
                    foreach (var oc in outputDataSet.Columns)
                    {
                        if (_helper.DoesRuleExist(ds, Array.IndexOf(outputDataSet.Columns, oc)))
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

            outputDataSet = _helper.MapSets(dataSets, outputDataSet);

            Console.WriteLine("\nData is ready to be coppied into file format.");

            while (true)
            {
                Console.WriteLine("\nPlease input the Directory where you would like the file to be output.");
                var outPutDirectory = Console.ReadLine();
                if (!Directory.Exists(outPutDirectory))
                {
                    Console.Beep();
                    Console.WriteLine("\nInvalid Directory." +
                        "\nPlease ensure that the directoy exists and is spelled correctly.");
                }
                else
                {
                    outputDataSet.InputFilePath = outPutDirectory;
                    _helper.MakeFile(outputDataSet);
                    break;
                }
            }

            Console.WriteLine("File Successfully created\n" +
                "\n" +
                "Press any key to terminate.");
            Console.ReadKey();
            System.Environment.Exit(1);
        }
    }
}
