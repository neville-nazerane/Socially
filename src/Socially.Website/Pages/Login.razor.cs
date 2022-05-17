using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Exceptions;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;

namespace Socially.Website.Pages
{
    public partial class Login
    {

        ICollection<string> errors = new List<string>();

        LoginModel loginModel;
        SignUpModel signUpModel;

        [Inject]
        public IApiConsumer Consumer { get; set; }

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
            try
            {
                var res = await Consumer.LoginAsync(loginModel);
                Console.WriteLine(res);
            }
            catch (ErrorForClientException ex)
            {
                errors = ex.Errors.SelectMany(e => e.Errors).ToList();
            }
        }

    }
}
