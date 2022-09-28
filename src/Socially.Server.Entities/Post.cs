using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Entities
{
    public class Post
    {

        public Guid Id { get; set; }

        [Required]
        public DateTime? CreatedOn { get; set; }

        [Required]
        public int? CreatorId { get; set; }
        public User Creator { get; set; }

    }
}
