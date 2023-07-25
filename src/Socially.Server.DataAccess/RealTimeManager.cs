﻿using Microsoft.EntityFrameworkCore;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public class RealTimeManager
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
            var entities = postIds.Select(p => new PostRealTime
            {
                PostId = p,
                ConnectionId = connectionId
            }).ToList();
            await _dbContext.Posts.AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }        

        public Task UnsubscribeForConnectionAsync(string connectionId,
                                                  CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts
                                .Where(p => p.ConnectionId == connectionId)
                                .ExecuteDeleteAsync(cancellationToken);
        }

    }
}
