namespace System
{
    public static class StringExtensions
    {

        public static string LowerFirstLetter(this string str)
            => str[0].ToString().ToLower() + str[1..];

        public static string UpperFirstLetter(this string str)
            => str[0].ToString().ToUpper() + str[1..];

        public static string ClearNewLines(this string str)
            => str.Replace("\n", string.Empty)
                  .Replace("\r", string.Empty)
                  .Replace("\t", string.Empty)
                  .Trim();

        public static string GetFullName(this Type type)
        {
            string fullName = type.FullName; // $"{type.Namespace}.{type.Name.Substring(0, type.Name.IndexOf("`"))}"; 
            if (type.IsGenericType)
            {
                fullName = fullName[..fullName.IndexOf("`")];
                var subTypes = type.GetGenericArguments()
                                   .Select(t => t.GetFullName())
                                   .ToArray();
                var gentypes = string.Join(", ", subTypes);
                fullName = $"{fullName}<{gentypes}>";
            }
            return fullName;
        }

    }
}
