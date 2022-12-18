using Socially.Apps.Consumer.Models;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Socially.MobileApp.Services
{
    public class AuthAccess : IAuthAccess
    {

        private const string key = "tokenData";
        StoredToken token;

        static TaskCompletionSource<StoredToken> locker;

        public async ValueTask<StoredToken> GetStoredTokenAsync()
        {
            if (locker is not null)
                return await locker.Task;

            if (token is null)
            {
                var str = await SecureStorage.GetAsync(key);
                if (str is not null)
                    token = JsonSerializer.Deserialize<StoredToken>(str);
            }

            return token;
        }

        public async ValueTask SetStoredTokenAsync(TokenResponseModel res)
        {
            if (locker is not null) await locker.Task;

            var localLock = new TaskCompletionSource<StoredToken>();
            locker = localLock;

            try
            {
                token = new()
                {
                    AccessToken = res.AccessToken,
                    RefreshToken = res.RefreshToken,
                    Expiary = DateTime.UtcNow.AddSeconds(res.ExpiresIn)
                };
                await SecureStorage.SetAsync(key, JsonSerializer.Serialize(token));
            }
            finally
            {
                locker = null;
                localLock.SetResult(token);
            }
        }
    }
}
