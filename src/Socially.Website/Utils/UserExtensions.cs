using Socially.Models;

namespace Socially.Website.Utils
{
    public static class UserExtensions
    {

        public static string GetDisplayName(this UserSummaryModel user)
        {
            if (string.IsNullOrEmpty(user.FirstName) && string.IsNullOrEmpty(user.LastName))
                return user.UserName;
            else
                return $"{user.FirstName} {user.LastName}";
        }

    }
}
