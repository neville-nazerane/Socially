using Socially.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Socially.Apps.Consumer.Exceptions
{
    public class ErrorForClientException : Exception
    {
        public IEnumerable<ErrorModel> Errors { get; set; }


        public ErrorForClientException(IEnumerable<ErrorModel> errors) : base(JsonSerializer.Serialize(errors))
        {
            Errors = errors;
        }

    }
}
