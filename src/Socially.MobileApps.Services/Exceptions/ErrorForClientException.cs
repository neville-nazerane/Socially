using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Socially.MobileApps.Services.Exceptions
{
    public class ErrorForClientException : Exception
    {
        public ErrorModel Error { get; set; }


        public ErrorForClientException(ErrorModel error) : base(JsonSerializer.Serialize(error))
        {
            Error = error;
        }

    }
}
