using Moq;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Tests
{
    public class RealtimeStateManagerTest : TestBase
    {
        private readonly Mock<ITableAccess> mockedTableAccess;
        private readonly RealtimeStateManager manager;

        public RealtimeStateManagerTest()
        {
            mockedTableAccess = new Mock<ITableAccess>();
            manager = new RealtimeStateManager(mockedTableAccess.Object);
        }

    }
}
