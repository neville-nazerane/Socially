using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Models
{
    public abstract class PublishMessage
    {
        public PublishType Type { get; }

        protected PublishMessage(PublishType type)
        {
            Type = type;
        }

    }

}
