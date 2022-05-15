using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Socially.Apps.Consumer
{
    public class ApiConsumer : IApiConsumer
    {
        private readonly HttpClient _httpClient;

        public ApiConsumer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    }
}
