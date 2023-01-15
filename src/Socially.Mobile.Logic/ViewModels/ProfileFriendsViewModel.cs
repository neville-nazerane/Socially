using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Socially.Apps.Consumer.Services;
using Socially.Mobile.Logic.Models;
using Socially.Mobile.Logic.Models.Mappings;
using Socially.Mobile.Logic.Services;
using Socially.Mobile.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.ViewModels;

public partial class ProfileFriendsViewModel : ViewModelBase
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

    [ObservableProperty]
    ObservableCollection<GroupedUsers> groupedData;

    public ProfileFriendsViewModel(ISocialLogger logger, IApiConsumer apiConsumer)
    {
        _logger = logger;
        _apiConsumer = apiConsumer;

        GroupedData = new();
    }

    partial void OnFriendRequestsChanged(ObservableCollection<UserSummaryModel> value) => GroupedData.Insert(1, new(value, "Friend Requests"));

    partial void OnFriendsChanged(ObservableCollection<UserSummaryModel> value) => GroupedData.Insert(0, new(value, "Friends"));

    public override Task OnNavigatedAsync() => RefreshAsync();

    [RelayCommand]
    Task RefreshAsync()
    {
        GroupedData.Clear();
        return Task.WhenAll(LoadFriendRequestAsync(), LoadFriendsAsync());
    }

    async Task LoadFriendRequestAsync()
    {
        IsFriendRequestsLoading = true;
        try
        {
            FriendRequests = new(await _apiConsumer.GetFriendRequestsAsync().ToMobileModel());
        }
        catch (Exception ex)
        {
            _logger.LogException(ex);
        }
        finally
        {
            IsFriendRequestsLoading = false;
        }
    }

    async Task LoadFriendsAsync()
    {
        IsFriendsLoading = true;
        try
        {
            Friends = new(await _apiConsumer.GetFriendsAsync().ToMobileModel());
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