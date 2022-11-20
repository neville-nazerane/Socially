using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System.Threading.Tasks;

namespace Socially.Website.Components
{
    public partial class AddPostComponent
    {

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public CachedContext CachedContext { get; set; }

        AddPostModel addPostModel = new();

        async Task AddPostAsyc()
        {
            if (string.IsNullOrEmpty(addPostModel.Text)) return;

            await Consumer.AddPostAsync(addPostModel);
            addPostModel = new AddPostModel();
            StateHasChanged();
        }


    }
}
