using Microsoft.AspNetCore.SignalR;
using Socially.Server.DataAccess;
using Socially.Server.Managers.Utils;
using Socially.Server.Managers;
using Socially.WebAPI.Utils;
using System;
using Azure.Core;
using System.Threading.Tasks;

namespace Socially.WebAPI.Services
{
    class HubScope : ScopableServiceProvider
    {

        private readonly Hub _source;
        private readonly Guid? _requestId;

        public IPostManager PostManager => GetService<IPostManager>();

        public IRealTimeManager RealTimeManager => GetService<IRealTimeManager>();

        public HubScope(IServiceProvider serviceProvider, Hub source, Guid? requestId) : base(serviceProvider)
        {
            var currentContext = GetService<CurrentContext>();
            source.Context.User.Populate(currentContext);
            _source = source;
            _requestId = requestId;
        }

        public Task SendErrorAsync(string errorMessage)
            => _source.Clients.Client(_source.Context.ConnectionId)
                              .SendAsync("ErrorOccurred", _requestId, errorMessage);

        public override async ValueTask DisposeAsync()
        {
            if (!_disposed && _requestId is not null)
                await _source.Clients.Client(_source.Context.ConnectionId).SendAsync("Completed", _requestId);
            await base.DisposeAsync();
        }

    }
}
