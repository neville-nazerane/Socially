﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Core.Models
{
    public class ImageUploadModel
    {

        public IFormFile Image { get; set; }

    }
}
