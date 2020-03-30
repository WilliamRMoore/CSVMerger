using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace CsvMerger
{
    class Program
    {

        public static bool ValidateSets(string filePath, string[] sets)
        {
            List<string> pathsToMergeFiles = new List<string>();
            foreach (var s in sets)
            {
                var pathOfSet = filePath + $@"\{s}";
                if (!File.Exists(pathOfSet))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool DoesRuleExist(DataSet set, int rule)
        {
            return set.MapRules.Any(r => r[1] == rule);
        }

        public static DataSet MapSets(List<DataSet> dataSets, DataSet ResultDataSet)
        {
            decimal rowCount = 0.0m;
            decimal rowProccessed = 0.0m;
            decimal percentDone = 0.0m;

            foreach (var ds in dataSets)
            {
                rowCount += ds.Rows.Length;
            }

            foreach (var ds in dataSets)
            {
                foreach (var row in ds.Rows)
                {
                    string[] rowArray = new string[ResultDataSet.Columns.Length];
                    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    var attributes = CSVParser.Split(row);

                    for (int i = 0; i < attributes.Length; i++)
                    {
                        var rules = ds.MapRules.Select(r => r).Where(r => r[0] == i);
                        if (rules.Any())
                        {
                            foreach (var r in rules)
                            {
                                rowArray[r[1]] = attributes[i];
                            }
                        }
                    }

                    ResultDataSet.OutputRows.Add(string.Join(",", rowArray));
                    rowProccessed += 1;
                    var previousPercent = percentDone;
                    percentDone = Math.Floor((rowProccessed / rowCount) * 100);
                    if (percentDone > previousPercent)
                    {
                        Console.WriteLine($"{percentDone}% Done");
                    }
                }
            }

            return ResultDataSet;
        }


        public static List<DataSet> LoadDataSets(List<DataSet> dataSets)
        {
            foreach (var ds in dataSets)
            {
                ds.Columns = File.ReadLines(ds.FilePath).First().Split(",");
                ds.Rows = File.ReadLines(ds.FilePath).Skip(1).ToArray();
            }

            return dataSets;
        }

        public static string FormatDataSetsList(IEnumerable<FileInfo> filenames)
        {
            string outputFileList = "";
            string format = "{0,-15} {1,15}";


            for (int i = 0; i < filenames.Count(); i = i + 2)
            {
                var filename1 = filenames.ElementAt(i).Name;
                string filename2 = "";
                if (i != filenames.Count() - 1)
                {
                    filename2 = filenames.ElementAt(i + 1).Name;
                }

                outputFileList += string.Format(format, filename1, filename2);
                outputFileList += "\n";
            }

            return outputFileList;
        }

        public static void MakeFile(DataSet set)
        {
            decimal rowCount = set.OutputRows.Count();
            decimal rowProccessed = 0.0m;
            decimal percentDone = 0.0m;

            try
            {
                using (StreamWriter output = new StreamWriter(set.FilePath + "\\" + set.FileName + ".csv"))
                {
                    output.WriteLine(String.Join(",", set.Columns));

                    foreach (var row in set.OutputRows)
                    {
                        var previousPercent = percentDone;
                        output.WriteLine(row);
                        rowProccessed += 1;

                        percentDone = Math.Floor((rowProccessed / rowCount) * 100);
                        if (percentDone > previousPercent)
                        {
                            Console.WriteLine($"{percentDone}% Done");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            DataSet outputDataSet = new DataSet();
            List<DataSet> dataSets = new List<DataSet>();
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

            Console.WriteLine("Welcome to the .arff generator tool, this tool is used to merge disparate data sets \n" +
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
                    DataSet dataSet = new DataSet();
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

            var fileList = FormatDataSetsList(filenames);

            Console.WriteLine(fileList);

            Console.WriteLine("If contents blank, please make sure to copy over the datasets you \n" +
                $"wish to merge into the DataSets folder located in {path + @"\DataSets"}.\n");

            Console.WriteLine("Please list the data sets you wish to merge seperated by commas.\n");


            do
            {
                var mergeSets = Console.ReadLine();

                var sets = mergeSets.Split(",");

                if (ValidateSets(dataSetsPath, sets))
                {
                    foreach (var s in sets)
                    {
                        DataSet dataSet = new DataSet();
                        dataSet.FileName = s;
                        dataSet.FilePath = dataSetsPath + $@"\{s}";

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

            dataSets = LoadDataSets(dataSets);

            foreach (var ds in dataSets)
            {
                Console.Clear();
                Console.WriteLine($"Please map which columns from dataset {ds.FileName} you want \n" +
                    $"to the correct column on {outputDataSet.FileName}.");
                foreach (var c in ds.Columns)
                {
                    foreach (var oc in outputDataSet.Columns)
                    {
                        if (DoesRuleExist(ds, Array.IndexOf(outputDataSet.Columns, oc)))
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

            outputDataSet = MapSets(dataSets, outputDataSet);

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
                    outputDataSet.FilePath = outPutDirectory;
                    MakeFile(outputDataSet);
                    break;
                }
            }

            Console.WriteLine("File Successfully created\n" +
                "\n" +
                "Press any key to terminate.");
            Console.ReadLine();
            System.Environment.Exit(1);
        }
    }
}
