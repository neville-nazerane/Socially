using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Pages
{
    public partial class Index
    {

        private ICollection<PostDisplayModel> posts;

        [Inject]
        public IApiConsumer Consumer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            posts = (await Consumer.GetHomePostsAsync(20)).ToList();
        }

    }
}
