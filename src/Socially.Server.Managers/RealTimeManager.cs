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
            => _dbContext.PostConnections
                                .Where(p => p.ConnectionId == connectionId)
                                .ExecuteDeleteAsync(cancellationToken);

        public async Task SubscribeForUsersAsync(string connectionId,
                                         IEnumerable<int> userIds,
                                         CancellationToken cancellationToken = default)
        {
            var entities = userIds.Select(u => new UserConnection
            {
                UserId = u,
                ConnectionId = connectionId
            }).ToList();

            await _dbContext.UserConnections.AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IAsyncEnumerable<string> GetUserConnectionIdsAsync(int userId)
            => _dbContext.UserConnections
                          .Where(u => u.UserId == userId)
                          .Select(u => u.ConnectionId)
                          .AsAsyncEnumerable();

        public Task UnsubscribeForUserConnectionAsync(string connectionId,
                                                      CancellationToken cancellationToken = default) 
            => _dbContext.UserConnections
                                .Where(u => u.ConnectionId == connectionId)
                                .ExecuteDeleteAsync(cancellationToken);

    }
}
