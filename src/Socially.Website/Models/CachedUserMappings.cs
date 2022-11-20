using Socially.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace Socially.Website.Models
{

    public class CachedUserMappings
    {

        readonly ConcurrentDictionary<int, UserSummaryModel> _users = new();
        readonly ConcurrentBag<int> _initialized = new();

        public UserSummaryModel Get(int id) => _users.GetOrAdd(id, id => new());

        public IEnumerable<int> ExistingIds => _users.Keys;

        public void Update(IEnumerable<UserSummaryModel> users)
        {
            foreach (var user in users)
            {
                lock (_initialized)
                {
                    if (!_initialized.Contains(user.Id))
                        _initialized.Add(user.Id);
                }
                _users.AddOrUpdate(user.Id, user, (id, existing) =>
                {
                    existing.FirstName = user.FirstName;
                    existing.LastName = user.LastName;
                    existing.ProfilePicUrl = user.ProfilePicUrl;

                    return existing;
                });
            }
        }

        public bool IsInitialized(params int[] ids)
            => _initialized.Intersect(ids).Count() == ids.Count();

    }
}
