namespace System
{
    public static class DateExtensions
    {

        public static string ToDuration(this DateTime dataTime)
        {
            var span = DateTime.UtcNow - dataTime;
            if (span.TotalDays > 1)
                return $"{span.Days} days ago";
            if (span.TotalHours > 1)
                return $"{span.Hours} hours ago";
            if (span.Minutes > 1)
                return $"{span.Minutes} minutes ago";
            else return $"{span.Seconds} seconds ago";
        }

        public static string ToDuration(this DateTime? dataTime) => dataTime?.ToDuration();

    }
}
