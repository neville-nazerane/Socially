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

        public Dictionary<string, IEnumerable<string>> Errors { get; set; }

    }
}
