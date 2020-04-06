using CsvMerger.Services.ServiceLayers.TuiInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvMerger.Services.ServiceLayers.TuiRoutines.InitialDataInput
{
    public interface IInitialDataInput
    {
        void Welcome();
        void PrintDataSetNameCursor();
        string GetNewDatasetName();
        string[] GetColumnNames();
        bool ValidateNewDatasetName(string newDataSetName);
        void AskUserIfDataIsCorrect(string fileName, string columnNames);
        string GetTargetDirectory();
    }

    public class InitialDataInput  : IInitialDataInput
    {
        private readonly IUXTextOutput uXTextOutput;
        private readonly IUserInputValidator userInputValidator;

        public InitialDataInput(IUXTextOutput uXTextOutput, IUserInputValidator userInputValidator)
        {
            this.uXTextOutput = uXTextOutput;
            this.userInputValidator = userInputValidator;
        }

        public void Welcome()
        {
            uXTextOutput.OutputStringNewLine("Welcome to the .csv generator tool, this tool is used to merge disparate data sets \n" +
                "into a .arff file for use in the Weka data analysis program.\n" +
                "\n Please specify the name of the dataset you would like to create \n" +
                "***WARNING DO NOT INCLUDE FILE EXTENSION IN NAME***\n" +
                "***WARNING DO NOT INCLUDE THE FOLLOWING CHARACTERS IN THE FILE NAME***\n" +
                "* . \" / \\ [ ] : ; | ,\n");
        }

        public void PrintDataSetNameCursor()
        {
            uXTextOutput.OutputStringSameLine("New DataSet name --> ");
        }

        public string GetNewDatasetName()
        {
            do
            {
                PrintDataSetNameCursor();
                string newdataSetName = Console.ReadLine();
                var valid = ValidateNewDatasetName(newdataSetName);

                if (valid)
                {
                    return newdataSetName;
                }
                else
                {
                    uXTextOutput.OutputStringNewLine("Please make sure your dataset name does not contain illegal characters.\n");
                }

            } while (true);
        }

        public string[] GetColumnNames()
        {
            uXTextOutput.OutputStringNewLine("\nPlease List the Column Names seperated by commas.");
            var columnNames = Console.ReadLine();
            var colunmArr = columnNames.Split(",");

            return colunmArr;
        }

        public bool ValidateNewDatasetName(string newDataSetName)
        {
            Regex illegaleCharsRegex = new Regex(@"^[a-zA-Z0-9_]+$");
            if (!illegaleCharsRegex.IsMatch(newDataSetName))
            {
                return false;
            }

            return true;
        }

        public void AskUserIfDataIsCorrect(string fileName, string columnNames)
        {
            uXTextOutput.OutputStringNewLine($"\nA data set with the name of {fileName} containing the columns {columnNames} \n" +
                        $"will be generated. Do you wish to continue?");
            uXTextOutput.OutputStringNewLine("\n[Y/N]?");

            Func<string, bool> validateYN = s =>
            {
                return s.Equals("Y") ? true : false;
            };

            Func<string, bool> validateInput = s =>
            {
                bool res = (s.Equals("Y") || s.Equals("N"));
                return res;
            };  

            while (true)
            {
                var input = Console.ReadLine().ToUpper();
                if(!userInputValidator.ValidateInput(input, validateInput))
                {
                    uXTextOutput.OutputStringNewLine("\nPlease select Y or N.");
                    continue;
                }

                if(userInputValidator.ValidateInput(input, validateYN))
                {
                    break;
                }
                else
                {
                    System.Environment.Exit(1);
                }
            }
        }

        public string GetTargetDirectory()
        {
            uXTextOutput.OutputStringNewLine("\nPlease input the directory where you would like the to be output.");

            Func<string, bool> DirectoyExists = s =>
            {
                return Directory.Exists(s);
            };

            while (true)
            {
                var outputDirectory = Console.ReadLine();

                if(userInputValidator.ValidateInput(outputDirectory, DirectoyExists))
                {
                    return outputDirectory;
                }
                else
                {
                    uXTextOutput.Beep();
                    uXTextOutput.OutputStringNewLine("Invalid Directory." +
                        "\nPlease ensure that the directoy exists and is spelled correctly.");
                }

            }
        }
    }
}
