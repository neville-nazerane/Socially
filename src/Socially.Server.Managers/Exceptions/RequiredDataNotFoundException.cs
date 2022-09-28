using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Exceptions
{
    public class RequiredDataNotFoundException : Exception
    {
        public RequiredDataNotFoundException(object id, string title)
            : base ($"The data {title} with id {id} was not found")
        {

        }

    }
}
