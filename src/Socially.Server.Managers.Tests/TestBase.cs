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
        public RealTimeDbContext RealTimeDbContext { get; }

        public CurrentContext CurrentContext { get; }

        public TestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                .Options;

            var optionsReal = new DbContextOptionsBuilder<RealTimeDbContext>()
                                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                    .Options;
            DbContext = new(options);
            RealTimeDbContext = new(optionsReal);
            CurrentContext = new CurrentContext();
        }

    }
}
