using Socially.Models;
using Socially.Server.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.ModelMappings
{
    public static class UserMappingExtensions
    {

        public static IQueryable<UserSummaryModel> SelectAsSummaryModel(this IQueryable<User> users)
            => users.Select(u => new UserSummaryModel
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ProfilePicUrl = u.ProfilePicture.FileName
            });

    }
}
