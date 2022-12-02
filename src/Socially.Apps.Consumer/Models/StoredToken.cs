using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Models
{
    public class StoredToken
    {

        public string AccessToken { get; set; }

        public DateTime Expiary { get; set; }

        public string RefreshToken { get; set; }


    }
}
