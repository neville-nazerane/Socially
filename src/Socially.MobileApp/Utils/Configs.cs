using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Utils
{
    public partial class Configs
    {

        public static string BaseURL { get; }
        public static string ImageBase { get; }

#if RELEASE
        static Configs()
        {
            BaseURL = "https://api.sociallyconnections.com";
            ImageBase = "https://pics.sociallyconnections.com";
        }
#endif

    }
}
