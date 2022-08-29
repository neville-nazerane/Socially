using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Account
{
    public partial class ForgotPassword
    {

        ForgotPasswordModel model = new();

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string Token { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string UserName { get; set; }

        protected override void OnInitialized()
        {
            model.Token = Token;
            model.UserName = UserName;
        }

        async Task SubmitAsync()
        {
            var res = await Consumer.ResetForgottenPasswordAsync(model);
            res.EnsureSuccessStatusCode();
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    model.Token = Token;
        //    model.UserName = UserName;

        //    //await Consumer.ResetForgottenPasswordAsync(new Core.Models.ForgotPasswordModel
        //    //{
        //    //    Token = Token,
        //    //    UserName = UserName
        //    //});
        //}

    }
}
