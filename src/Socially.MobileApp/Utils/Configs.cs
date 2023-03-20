using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Utils
{
    public static partial class Configs
    {
        private static string _noSqlLocation;

        public static string BaseURL { get; }
        public static string ImageBase { get; }

        public static string AppCenter { get; }

        public static string NoSqlLocation => _noSqlLocation ??= $"{FileSystem.Current.AppDataDirectory}/nosql.db";

#if RELEASE
        static Configs()
        {
            BaseURL = "https://api.sociallyconnections.com";
            ImageBase = "https://pics.sociallyconnections.com";
        }
#endif

    }
}
