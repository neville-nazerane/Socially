using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Core.Models
{
    public class UploadContext
    {

        public Stream Stream { get; set; }

        public string FileName { get; set; }

    }
}
