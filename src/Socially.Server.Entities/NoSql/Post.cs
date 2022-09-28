using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Socially.Server.Entities.NoSql
{
    public class Post
    {

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatorId { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public object comments { get; set; }

        public Post()
        {
            Comments = Array.Empty<Comment>();
        }

    }
}
