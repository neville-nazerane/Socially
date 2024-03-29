﻿using CommunityToolkit.Mvvm.ComponentModel;
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

    partial void OnFriendRequestsChanged(ObservableCollection<UserSummaryModel> value) 
        => GroupedData.Insert(1, new(value.Select(v => new DetailedUser(UserType.Request, v)), "Friend Requests"));

    partial void OnFriendsChanged(ObservableCollection<UserSummaryModel> value)
        => GroupedData.Insert(0, new(value.Select(v => new DetailedUser(UserType.Friend, v)), "Friends"));

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

    [RelayCommand]
    async Task RequestFriendAsync(int forUserId, CancellationToken cancellationToken = default)
    {
        await _apiConsumer.RequestFriendAsync(forUserId, cancellationToken);
        await RefreshAsync();
    }

    [RelayCommand]
    async Task AcceptFriendRequestAsync(int userId,  CancellationToken cancellationToken = default)
    {
        await _apiConsumer.RespondToFriendRequestAsync(userId, true, cancellationToken);
        await RefreshAsync();
    }

    [RelayCommand]
    async Task RejectFriendRequestAsync(int userId, CancellationToken cancellationToken = default)
    {
        await _apiConsumer.RespondToFriendRequestAsync(userId, false, cancellationToken);
        await RefreshAsync();
    }

    [RelayCommand]
    async Task RemoveFriendAsync(int userId, CancellationToken cancellationToken = default)
    {
        await _apiConsumer.RemoveFriendAsync(userId, cancellationToken);
        await RefreshAsync();
    }

}