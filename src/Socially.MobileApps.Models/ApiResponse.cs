using System;
using System.Collections.Generic;
using System.Text;

namespace Socially.MobileApps.Models
{

    public class ApiResponse 
    {
        public bool IsSuccess { get; set; }

        public int Status { get; set; }

        public Dictionary<string, IEnumerable<string>> Errors { get; set; }

    }

    public class ApiResponse<T> : ApiResponse
    {

        public T Data { get; set; }

    }

}
