using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Socially.Apps.Consumer.Services;
using Socially.Models;
using Socially.Website.Services;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace Socially.Website.Pages.Account
{

    public partial class EditAccount
    {

        [Inject]
        public IConfiguration Config { get; set; }


        [Inject]
        public IApiConsumer ApiConsumer { get; set; }

        [Inject]
        public SignalRListener SignalRListener { get; set; }


        bool showImages = false;

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
        Guid? updatingRequestId = null;
        bool isPasswordResetting = false;


        protected override async Task OnInitializedAsync()
        {
            SignalRListener.OnCompleted += OnCompleted;
            model = await ApiConsumer.GetUpdateProfileAsync();
            await base.OnInitializedAsync();
        }

        private void OnCompleted(object sender, Models.RealtimeEventArgs.CompletedEventArgs e)
        {
            if (updatingRequestId == e.RequestId)
            {
                isAccountUpdating = false;
                StateHasChanged();
            }
        }

        async Task UpdateAsync()
        {
            isAccountUpdating = true;
            try
            {
                updatingRequestId = await SignalRListener.UpdateUserAsync(model);
            }
            catch (Exception ex)
            {
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

        void SwapShowImage() => showImages = !showImages;

        void ClearProfilePic()
        {
            model.ProfilePictureFileName = null;
            StateHasChanged();
        }

        Task ImageSelectedAsync(string filename)
        {
            model.ProfilePictureFileName = filename;
            showImages = false;
            StateHasChanged();
            return Task.CompletedTask;
        }

    }
}
