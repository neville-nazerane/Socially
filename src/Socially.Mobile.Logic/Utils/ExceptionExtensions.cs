using Socially.Apps.Consumer.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Utils
{
    public static class ExceptionExtensions
    {

        public static IEnumerable<ValidationResult> ToEnumerable(this ErrorForClientException exception)
            => exception.Errors
                        .SelectMany(f => f.Errors
                        .Select(e => new ValidationResult(e, new[] { f.Field }))
                        .ToArray());

        public static ObservableCollection<ValidationResult> ToObservableCollection(this ErrorForClientException exception)
            => new(exception.ToEnumerable());

    }
}
