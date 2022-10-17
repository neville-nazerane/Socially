using Socially.Apps.Consumer.Exceptions;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Utils
{
    public class ApiHttpHandler : HttpClientHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errors = await response.Content.ReadFromJsonAsync<IEnumerable<ErrorModel>>();
                throw new ErrorForClientException(errors);
            }

            return response;
        }

    }
}
