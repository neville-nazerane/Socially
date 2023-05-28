using Azure.Data.Tables;
using Socially.Server.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers
{
    public class SignalRStateManager : ISignalRStateManager
    {

        private const string LISTENER_TABLE_NAME = "signalRListeners";
        private const string LISTENER_REVERSE_TABLE_NAME = "signalRListenersReverse";

        private readonly ITableAccess _tableAccess;

        public SignalRStateManager(ITableAccess tableAccess)
        {
            _tableAccess = tableAccess;
        }

        public async Task InitAsync(CancellationToken cancellationToken = default)
        {
            await _tableAccess.CreateTableIfNotExistAsync(LISTENER_TABLE_NAME, cancellationToken);
            await _tableAccess.CreateTableIfNotExistAsync(LISTENER_REVERSE_TABLE_NAME, cancellationToken);
        }

        public async Task RegisterAsync(IEnumerable<string> listenerTags,
                                        string connectionId,
                                        CancellationToken cancellationToken = default)
        {
            var entities = listenerTags.Select(l => new TableEntity(l, connectionId))
                                       .ToList();
            await _tableAccess.AddRangeAsync(LISTENER_TABLE_NAME, entities, cancellationToken);

            foreach (var l in listenerTags)
                await _tableAccess.AddAsync(LISTENER_REVERSE_TABLE_NAME, new TableEntity(connectionId, l), CancellationToken.None);
        }

        public async Task UnregisterAsync(string connectionId)
        {
            var deletes = _tableAccess.ListByPartitionAsync(LISTENER_REVERSE_TABLE_NAME, connectionId, 100);
            await foreach (var toDelete in deletes)
                await _tableAccess.DeleteAsync(LISTENER_TABLE_NAME, connectionId, toDelete.RowKey, CancellationToken.None);
            await _tableAccess.DeleteAllWithPartitionAsync(LISTENER_REVERSE_TABLE_NAME, connectionId);
        }

        public async IAsyncEnumerable<string> GetConnectionIdsAsync(string listenerTag,
                                                                    int pageSize)
        {
            var elements = _tableAccess.ListByPartitionAsync(LISTENER_TABLE_NAME, listenerTag, pageSize);
            await foreach (var element in elements)
                yield return element.RowKey;
        }

    }
}
