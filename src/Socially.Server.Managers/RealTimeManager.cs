using Microsoft.EntityFrameworkCore;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public class RealTimeManager : IRealTimeManager
    {
        private readonly RealTimeDbContext _dbContext;

        public RealTimeManager(RealTimeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SubscribeForPostsAsync(string connectionId,
                                                 IEnumerable<int> postIds,
                                                 CancellationToken cancellationToken = default)
        {
            var entities = postIds.Select(p => new PostConnection
            {
                PostId = p,
                ConnectionId = connectionId
            }).ToList();

            await _dbContext.PostConnections.AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IAsyncEnumerable<string> GetPostConnectionIdsAsync(int postId) 
            => _dbContext.PostConnections
                          .Where(p => p.PostId == postId)
                          .Select(p => p.ConnectionId)
                          .AsAsyncEnumerable();

        public Task UnsubscribeForConnectionAsync(string connectionId,
                                                  CancellationToken cancellationToken = default)
        {
            return _dbContext.PostConnections
                                .Where(p => p.ConnectionId == connectionId)
                                .ExecuteDeleteAsync(cancellationToken);
        }

    }
}
