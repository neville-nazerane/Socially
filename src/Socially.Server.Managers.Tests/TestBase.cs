using Microsoft.EntityFrameworkCore;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Tests
{
    public class TestBase
    {

        public ApplicationDbContext DbContext { get; set; }

        public TestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                                .UseInMemoryDatabase("mainDb")
                                                .Options;
            DbContext = new ApplicationDbContext(options);
        }

    }
}
