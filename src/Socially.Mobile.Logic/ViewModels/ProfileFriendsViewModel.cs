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

namespace Socially.Mobile.Logic.ViewModels;

public partial class ProfileFriendsViewModel : ViewModelBase<ObservableCollection<UserSummaryModel>>
{
    private readonly ISocialLogger _logger;
    private readonly IApiConsumer _apiConsumer;

    public ProfileFriendsViewModel(ISocialLogger logger, IApiConsumer apiConsumer)
    {
        _logger = logger;
        _apiConsumer = apiConsumer;
    }

    public override void OnException(Exception ex) => _logger.LogException(ex);

    public override Task OnNavigatedAsync() => GetAsync();

    public override async Task<ObservableCollection<UserSummaryModel>> GetFromServerAsync(CancellationToken cancellationToken = default)
        => new(await _apiConsumer.GetFriendsAsync(cancellationToken).ToMobileModel());

}
