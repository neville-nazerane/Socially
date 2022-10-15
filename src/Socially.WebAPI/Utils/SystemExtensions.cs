namespace System
{
    public static class SystemExtensions
    {

        public static DateTime? ToDateTime(this string str)
            => string.IsNullOrEmpty(str) ? null : DateTime.Parse(str);

    }
}
