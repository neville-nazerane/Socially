using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Socially.Website.Pages
{
    public partial class Index
    {

        private IEnumerable<PostDisplayModel> posts;

        [Inject]
        public IApiConsumer Consumer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            posts = await Consumer.GetHomePostsAsync(20);
        }

    }
}
