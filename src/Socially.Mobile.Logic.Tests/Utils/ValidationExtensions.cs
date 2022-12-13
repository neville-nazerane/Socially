
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Tests.Utils
{
    public static class ValidationExtensions
    {

        public static bool IsValidProperty(this ICollection<ValidationResult> results, string propertyName)
            => results.All(r => r.MemberNames?.Contains(propertyName) != true);

        public static IEnumerable<string> GetErrorsForProperty(this ICollection<ValidationResult> results, string peopertyName)
            => results.Where(r => r.MemberNames.Contains(peopertyName))
                      .Select(r => r.ErrorMessage)
                      .ToArray();

    }
}
