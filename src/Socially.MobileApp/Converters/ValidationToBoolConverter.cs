using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Renderscripts.ScriptGroup;

namespace Socially.MobileApp.Converters
{
    public class FailedValidationToBoolConverter : ValueConverterBase<IEnumerable<ValidationResult>, bool, string>
    {

        protected override bool Convert(IEnumerable<ValidationResult> validation, string fieldName)
            => validation.Any(t => t.MemberNames.Contains(fieldName));

        protected override IEnumerable<ValidationResult> ConvertBack(bool response, string parameter)
            => throw new NotImplementedException("UI should not control validation");

    }
}
