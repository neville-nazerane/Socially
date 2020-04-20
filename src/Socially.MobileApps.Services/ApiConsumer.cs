using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Socially.MobileApps.Services
{
    public class ApiConsumer
    {
        private readonly HttpClient _client;

        private const string accountPath = "api/account";

        public ApiConsumer(HttpClient client)
        {
            _client = client;
        }

        

    }
}
