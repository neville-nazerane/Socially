using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;
using System;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Account
{

    public partial class EditAccount
    {
        ProfileUpdateModel model = null;

        PasswordResetModel passwordModel = new();

        bool StoreDateOfBirth
        {
            get => model?.DateOfBirth is not null;
            set
            {
                if (value)
                    model.DateOfBirth = DateTime.Now;
                else
                    model.DateOfBirth = null;
            }
        }

        DateTime? DateOfBirth
        {
            get => model?.DateOfBirth;
            set
            {
                if (value is not null)
                    model.DateOfBirth = value;
            }
        }

        bool isAccountUpdating = false;
        bool isPasswordResetting = false;

        [Inject]
        public IApiConsumer ApiConsumer { get; set; }

        protected override async Task OnInitializedAsync()
        {
            model = await ApiConsumer.GetUpdateProfileAsync();
            await base.OnInitializedAsync();
        }

        async Task UpdateAsync()
        {
            isAccountUpdating = true;
            try
            {
                await ApiConsumer.UpdateProfileAsync(model);
            }
            finally
            {
                isAccountUpdating = false;
            }
        }

        async Task ResetPasswordAsync()
        {
            isPasswordResetting = true;
            try
            {
                await ApiConsumer.ResetPasswordAsync(passwordModel);
                passwordModel = new();
            }
            finally
            {
                isPasswordResetting = false;
            }
        }

    }
}
