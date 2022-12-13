using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Services
{
    public interface ISocialLogger
    {

        void LogException(Exception ex, string message = null);

    }
}
