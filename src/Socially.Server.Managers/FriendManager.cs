﻿using DnsClient.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Socially.Models;
using Socially.Server.DataAccess;
using Socially.Server.Entities;
using Socially.Server.Managers.Exceptions;
using Socially.Server.ModelMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{

    public class FriendManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<FriendManager> _logger;

        public FriendManager(ApplicationDbContext dbContext, ILogger<FriendManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task RequestAsync(int requesterId, int approverId, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.FriendRequests.AsNoTracking()
                                                            .Where(
                                                                f => (f.RequesterId == requesterId && f.ForId == approverId
                                                                        || f.RequesterId == approverId && f.ForId == requesterId) && f.IsAccepted != false)
                                                            .Select(f => new
                                                            {
                                                                f.RequesterId,
                                                                f.RequestedOn
                                                            })
                                                            .SingleOrDefaultAsync(cancellationToken);
            if (existing is not null)
            {
                throw new FriendRequestExistsException(new FriendRequestExistsErrorModel
                {
                    RequesterId = existing.RequesterId.Value,
                    RequestedOn = existing.RequestedOn.Value
                });
            }

            await _dbContext.FriendRequests.AddAsync(new FriendRequest
            {
                ForId = approverId,
                RequesterId = requesterId,
                RequestedOn = DateTime.UtcNow
            }, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

        }

        public async Task<bool> RespondAsync(int requesterId,
                                             int forId,
                                             bool isAccepted,
                                             CancellationToken cancellationToken = default)
        {
            var existingRequest = await _dbContext.FriendRequests.SingleOrDefaultAsync(
                                                                f => f.RequesterId == requesterId && f.ForId == forId && f.IsAccepted != false,
                                                                cancellationToken);

            if (existingRequest is null)
            {
                _logger.LogWarning("Request doesnt exist for {requesterId} and for {forId}", requesterId, forId);
                return false;
            }
            existingRequest.IsAccepted = isAccepted;

            if (isAccepted)
            {
                await _dbContext.Friends.AddRangeAsync(new Friend[]
                {
                    new Friend
                    {
                        FriendUserId = requesterId,
                        OwnerUserId = forId
                    },
                    new Friend
                    {
                        FriendUserId = forId,
                        OwnerUserId = requesterId,
                    }
                },
                cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IEnumerable<UserSummaryModel>> SearchNonFriendsAsync(int userId,
                                                                               string query,
                                                                               CancellationToken cancellationToken = default)
            => await _dbContext.Users.Where(u => u.FirstName.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                                                || u.LastName.Contains(query, StringComparison.InvariantCultureIgnoreCase))
                                     .Where(u => !_dbContext.Users.Single(u => u.Id == userId).Friends.Any(f => f.FriendUserId == u.Id))
                                     .SelectAsSummaryModel()
                                     .ToListAsync(cancellationToken);

        public async Task<IEnumerable<UserSummaryModel>> GetRequestsAsync(int userId, CancellationToken cancellationToken = default)
            => await _dbContext.FriendRequests
                                 .AsNoTracking()
                                 .Where(r => r.ForId == userId && r.IsAccepted == null)
                                 .OrderBy(r => r.RequestedOn)
                                 .Select(r => new UserSummaryModel
                                 {
                                     Id = r.RequesterId.Value,
                                     FirstName = r.Requester.FirstName,
                                     LastName = r.Requester.LastName,
                                     ProfilePicUrl = r.Requester.ProfilePicture.FileName
                                 })
                                 .ToListAsync(cancellationToken);

        public async Task<IEnumerable<UserSummaryModel>> GetFriendsAsync(int userId, CancellationToken cancellationToken = default)
            => await _dbContext.Friends
                        .AsNoTracking()
                        .Where(r => r.OwnerUserId == userId)
                        .Select(r => new UserSummaryModel
                        {
                            Id = r.FriendUserId.Value,
                            FirstName = r.FriendUser.FirstName,
                            LastName = r.FriendUser.LastName,
                            ProfilePicUrl = r.FriendUser.ProfilePicture.FileName
                        })
                        .ToListAsync(cancellationToken);


    }
}