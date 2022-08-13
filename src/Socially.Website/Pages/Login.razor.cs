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

        bool isLoggingIn = false;
        bool isSigningUp = false;
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
            loginModel = new LoginModel
            {
                Source = "website"
            };
            signUpModel = new SignUpModel();
        }

        async Task SignupAsync()
        {
            isSigningUp = true;
            errors = Array.Empty<string>();
            try
            {
                await Consumer.SignupAsync(signUpModel);

                isSigningUp = false;
            }
            catch (ErrorForClientException ex)
            {
                errors = ex.Errors.SelectMany(e => e.Errors).ToList();
            }
            finally
            {
                isSigningUp = false;
            }
        }

        async Task LoginAsync()
        {
            isLoggingIn = true;
            errors = Array.Empty<string>();
            try
            {
                var res = await Consumer.LoginAsync(loginModel);
                await ((AuthProvider)AuthProvider).SetAsync(res.AccessToken);
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
            finally
            {
                isLoggingIn = false;
            }
        }

    }
}
