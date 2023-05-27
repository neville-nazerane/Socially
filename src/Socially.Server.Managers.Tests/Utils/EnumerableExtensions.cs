using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Tests.Utils
{
    internal static class EnumerableExtensions
    {

        public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
                yield return await ValueTask.FromResult(item);
        }


    }
}
