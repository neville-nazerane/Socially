using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models
{
    public interface IValidatable
    {
        bool Validate(ICollection<ValidationResult> errors);
    }
}
