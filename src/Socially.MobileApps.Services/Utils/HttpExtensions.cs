using Socially.MobileApps.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Http
{
    static class HttpExtensions
    {

        internal static async Task<ApiResponse<T>> CreateResponseAsync<T>(this HttpResponseMessage message)
        {
            var response = new ApiResponse<T>
            {
                IsSuccess = message.IsSuccessStatusCode
            };
            if (message.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(string))
                    response.Data = (T) (object) await message.Content.ReadAsStringAsync();
                response.Data = await message.Content.ReadFromJsonAsync<T>();
            }
            else if (message.StatusCode == HttpStatusCode.BadRequest)
            {
                response.Errors = await message.Content.ReadFromJsonAsync<Dictionary<string, IEnumerable<string>>>();
            }

            return response;
        }

    }
}
