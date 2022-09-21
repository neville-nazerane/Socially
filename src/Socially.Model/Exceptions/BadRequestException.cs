using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Core.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string defaultMessage = "Bad Request";

        public IEnumerable<ErrorModel> Errors { get; }

        public BadRequestException(string errorMessage, string message = defaultMessage) : base(message)
        {
            var model = new ErrorModel();
            model.Errors.Add(errorMessage);
            Errors = new[] { model };
        }

        public BadRequestException(string[] errorMessages, string message = defaultMessage) : base(message)
        {
            var model = new ErrorModel
            {
                Errors = errorMessages.ToList()
            };
            Errors = new[] { model };
        }

        public BadRequestException(ErrorModel errorMessages, string message = defaultMessage) : base(message)
        {
            Errors = new[] { errorMessages };
        }


        public BadRequestException(IEnumerable<ErrorModel> errors, string message = defaultMessage) : base(message)
        {
            Errors = errors;
        }


    }
}
