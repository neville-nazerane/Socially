using System.ComponentModel.DataAnnotations;

namespace Socially.Mobile.Logic.Models
{
    internal interface IValidatable
    {
        bool Validate(ICollection<ValidationResult> errors);
    }
}