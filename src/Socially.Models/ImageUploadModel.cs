using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class ImageUploadModel
    {

        public UploadContext ImageContext { get; set; }

        public IFormFile Image { get; set; }

    }
}
