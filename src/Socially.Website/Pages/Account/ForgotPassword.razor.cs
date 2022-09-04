using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Account
{
    public partial class ForgotPassword
    {

        ForgotPasswordModel model = new();

        string message;
        bool isDone = false;
        ICollection<string> errors = new List<string>();

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
            InitModel();
        }

        private void InitModel()
        {
            model = new()
            {
                Token = Token,
                UserName = UserName
            };
        }

        async Task SubmitAsync()
        {
            message = null;
            errors.Clear();
            try
            {
                var res = await Consumer.ResetForgottenPasswordAsync(model);
                res.EnsureSuccessStatusCode();
                InitModel();
                message = "DONE!";
                isDone = true;
            }
            catch (ErrorForClientException ex)
            {
                errors = ex.Errors.SelectMany(e => e.Errors).ToList();
            }
            catch
            {
                message = "Something went wrong";
            }
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
