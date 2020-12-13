using Socially.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.MobileApps.Models
{
    public class ApiResponse<T>
    {

        public T Data { get; set; }

        public bool IsSuccess { get; set; }

        public int Status { get; set; }

        public IEnumerable<ErrorModel> Errors { get; set; }

    }
}
