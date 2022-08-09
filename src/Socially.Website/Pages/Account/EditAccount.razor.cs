using Microsoft.AspNetCore.Components;
using Socially.Apps.Consumer.Services;
using Socially.Core.Models;

namespace Socially.Website.Pages.Account
{

    public partial class EditAccount
    {
        ProfileUpdateModel model = null;

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
            set {
                if (value is not null)
                    model.DateOfBirth = value;
            }
        }

        bool isAccountUpdating = false;

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

    }
}
