using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Socially.WebAPI.Models
{
    public class TokenRequest
    {

        public IEnumerable<Claim> Claims { get; set; }

        public string Audience { get; set; }

        public TimeSpan ExpireIn { get; set; }

    }
}
