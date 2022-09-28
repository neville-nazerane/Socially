using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities.NoSql
{
    public class Comment
    {

        public string Text { get; set; }

        public int PostId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatorId { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
        public Guid Id { get; set; }

        public Comment()
        {
            Comments = Array.Empty<Comment>();
        }
    }
}
