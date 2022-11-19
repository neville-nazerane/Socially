using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace Socially.Website.Components
{
    public partial class ProfilePicture
    {

        [Inject]
        public IConfiguration Config { get; set; }

        [Parameter]
        public string FileName { get; set; }
    }
}
