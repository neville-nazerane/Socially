using Socially.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    public interface IFriendsService
    {
        Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> GetRequestsAsync(CancellationToken cancellationToken = default);
        Task RequestAsync(int forId, CancellationToken cancellationToken = default);
        Task<bool> RespondAsync(int forId, bool isAccepted, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserSummaryModel>> SearchNonFriendsAsync(string query, CancellationToken cancellationToken = default);
    }
}