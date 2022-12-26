using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Converters
{
    public class FailedValidationToStringConverter : ValueConverterBase<IEnumerable<ValidationResult>, string, string>
    {

        protected override string Convert(IEnumerable<ValidationResult> validation, string fieldName)
            => validation.Where(t => t.MemberNames.Contains(fieldName))
                         .Select(t => t.ErrorMessage)
                         .FirstOrDefault(); // using first for now since UI only supports 1 error message

        protected override IEnumerable<ValidationResult> ConvertBack(string response, string parameter)
            => throw new NotImplementedException("UI should not control validation");

    }
}
