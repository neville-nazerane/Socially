using Socially.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Apps.Consumer.Services
{
    public interface ICacheUpdater
    {
        Task UpdatePostAsync(PostDisplayModel post);
    }
}
