using System;
using System.Collections.Generic;
using System.Text;

namespace CsvMerger.Services.ServiceLayers.TuiRoutines
{
    public interface IUserInputValidator
    {
        bool ValidateInput(string input, Func<string, bool> validator);
    }
    public class UserInputValidator : IUserInputValidator
    {
        public bool ValidateInput(string input, Func<string,bool> validator)
        {
            var result = validator(input);
            return result;
        }
    }
}
