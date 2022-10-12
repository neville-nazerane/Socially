using Microsoft.EntityFrameworkCore;
using Socially.Server.DataAccess;
using Socially.Server.Managers.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Tests
{
    public class TestBase
    {

        public ApplicationDbContext DbContext { get; }

        public CurrentContext CurrentContext { get; }

        public TestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                .Options;
            DbContext = new ApplicationDbContext(options);
            CurrentContext = new CurrentContext();
        }

    }
}
