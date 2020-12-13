using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.Core.Models
{
    public class ErrorModel
    {

        public string Field { get; set; }

        public ICollection<string> Errors { get; set; }

        public ErrorModel()
        {
            Errors = new List<string>();
        }

    }
}
