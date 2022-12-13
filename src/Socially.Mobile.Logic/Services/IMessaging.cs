using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Services
{
    public interface IMessaging
    {
        Task DisplayAsync(string title, string message, string button);
    }
}
