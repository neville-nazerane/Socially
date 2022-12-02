using Socially.Apps.Consumer.Models;
using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface IAuthAccess
    {

        ValueTask<StoredToken> GetStoredTokenAsync();

        ValueTask SetStoredTokenAsync(TokenResponseModel res);

    }
}
