using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Socially.WebAPI.IntegrationTests
{
    public class BasicTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_MustReturn200()
        {
            // ARRANGE
            var client = _factory.CreateClient();

            // ACT
            var response = await client.GetAsync("/");

            // ASSERT
            Assert.Equal(200, (int)response.StatusCode);

        }

        [Theory]
        [InlineData("/abc")]
        [InlineData("/nogo")]
        [InlineData("/abc/ddf")]
        public async Task Get_MustReturn404(string url)
        {
            // ARRANGE
            var client = _factory.CreateClient();

            // ACT
            var response = await client.GetAsync(url);

            // ASSERT
            Assert.Equal(404, (int)response.StatusCode);

        }

    }
}
