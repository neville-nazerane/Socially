using CommunityToolkit.Mvvm.ComponentModel;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ComponentModels
{
    public partial class ProfileFriendsComponentModel : ViewModelBase
    {
        private readonly ISocialLogger _logger;
        private readonly IApiConsumer _apiConsumer;

        [ObservableProperty]
        ObservableCollection<UserSummaryModel> friends;

        [ObservableProperty]
        ObservableCollection<UserSummaryModel> friendRequests;

        [ObservableProperty]
        bool isFriendsLoading;

        [ObservableProperty]
        bool isFriendRequestsLoading;

        public ProfileFriendsComponentModel(ISocialLogger logger, IApiConsumer apiConsumer)
        {
            _logger = logger;
            _apiConsumer = apiConsumer;
        }

        public override async Task OnNavigatedAsync()
        {
            await Task.WhenAll(LoadFriendRequestAsync(), LoadFriendsAsync());
        }

        async Task LoadFriendRequestAsync()
        {
            isFriendRequestsLoading = true;
            try
            {
                friendRequests = new(await _apiConsumer.GetFriendRequestsAsync().ToMobileModel());
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            finally
            {
                isFriendRequestsLoading = false;
            }
        }

        async Task LoadFriendsAsync()
        {
            IsFriendsLoading = true;
            try
            {
                friends = new(await _apiConsumer.GetFriendsAsync().ToMobileModel());
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
            finally
            {
                IsFriendsLoading = false;
            }
        }

    }
}
