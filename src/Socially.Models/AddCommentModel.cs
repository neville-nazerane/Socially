using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models
{
    public class AddCommentModel
    {

        public string Text { get; set; }

        public int PostId { get; set; }

        public Guid? ParentCommentId { get; set; }

    }
}
