using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{

    [Index(nameof(CreatedOn))]
    public class Post
    {

        public int Id { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [MaxLength(2000)]
        public string Text { get; set; }

        [Required]
        public int? CreatorId { get; set; }
        public User Creator { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
