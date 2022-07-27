using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;
using Socially.Website.Services;

namespace Socially.Website.Pages
{
    public partial class Login
    {

        ICollection<string> errors = new List<string>();

        LoginModel loginModel;
        SignUpModel signUpModel;

        [Inject]
        public IApiConsumer Consumer { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthProvider { get; set; }

        bool isSignup = false;

        void CleanUp()
        {
            errors = new List<string>();
        }

        public Login()
        {
            loginModel = new LoginModel();
            signUpModel = new SignUpModel();
        }

        async Task SignupAsync()
        {
            errors = Array.Empty<string>();
            try
            {
                await Consumer.SignupAsync(signUpModel);
            }
            catch (ErrorForClientException ex)
            {
                errors = ex.Errors.SelectMany(e => e.Errors).ToList();
            }
        }

        async Task LoginAsync()
        {
            errors = Array.Empty<string>();
            try
            {
                var res = await Consumer.LoginAsync(loginModel);
                await ((AuthProvider) AuthProvider).SetAsync(res);
                //await AuthService.SetAsync(res);
            }
            catch (ErrorForClientException ex)
            {
                errors = ex.Errors.SelectMany(e => e.Errors).ToList();
                bool hasErrors = errors.Any(e => !string.IsNullOrWhiteSpace(e));
                if (!hasErrors)
                {
                    errors = new string[] { "Invalid Login!" };
                }
            }
            catch
            {
                errors = new string[] { "failed to connect to server!" };
            }
        }

    }
}
