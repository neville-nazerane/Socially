using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.WebAPI.IntegrationTests
{
    public abstract class TestsBase : IClassFixture<CustomWebApplicationFactory>
    {

        const string testEmail = "ya@goo.com";
        const string testUsername = "username";
        const string testPassword = "pasSword!1";



    }
}
