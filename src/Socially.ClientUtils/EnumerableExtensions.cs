namespace Socially.ClientUtils
{
    public static class EnumerableExtensions
    {

        public static T[] ToSingleItemArray<T>(this T input)
            => new[] { input };

    }
}
