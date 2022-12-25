using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Utils.GeneratorCommon
{
    public static class FileUtils
    {

        public static string GrabPath(params string[] parts)
        {
            var path = Path.Combine(parts);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static string GrabPathAndClear(params string[] parts)
        {
            var path = GrabPath(parts);
            foreach (var file in Directory.GetFiles(path))
                File.Delete(file);

            return path;
        }


    }
}
