using Azure.Data.Tables;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public class TableAccess : ITableAccess
    {

        private readonly TableServiceClient _tableServiceClient;
        private readonly ConcurrentDictionary<string, TableClient> _clients;

        public TableAccess(string connString)
        {
            _tableServiceClient = new TableServiceClient(connString);
            _clients = new();
        }

        TableClient GetTableClient(string tableName)
            => _clients.GetOrAdd(tableName, t => _tableServiceClient.GetTableClient(tableName));

        public Task AddAsync(string tableName,
                             ITableEntity entity,
                             CancellationToken cancellationToken = default)
        {
            TableClient tableClient = GetTableClient(tableName);
            return tableClient.AddEntityAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(string tableName, IEnumerable<ITableEntity> entities, CancellationToken cancellationToken = default)
        {
            TableClient tableClient = GetTableClient(tableName);
            foreach (var chunk in GetActionChunks(entities, TableTransactionActionType.Add))
                await tableClient.SubmitTransactionAsync(chunk, cancellationToken);

        }

        static IEnumerable<IEnumerable<TableTransactionAction>> GetActionChunks(IEnumerable<ITableEntity> entities,
                                                                                TableTransactionActionType actionType)
        {
            var chunks = entities.GroupBy(e => e.PartitionKey)
                                  .SelectMany(g => g.Select(entity => new TableTransactionAction(actionType, entity))
                                                .Chunk(100))
                                  .ToList();
            return chunks;
        }

        public IAsyncEnumerable<TableEntity> ListByPartitionAsync(string tableName,
                                                                   string partitionKey,
                                                                   int pageSize)
            => ListByPartitionAsync<TableEntity>(tableName, partitionKey, pageSize);

        public IAsyncEnumerable<T> ListByPartitionAsync<T>(string tableName,
                                                           string partitionKey,
                                                           int pageSize)
            where T : class, ITableEntity, new()
        {
            TableClient tableClient = GetTableClient(tableName);
            var entities = tableClient.QueryAsync<T>(e => e.PartitionKey == partitionKey, maxPerPage: pageSize);
            return entities;
        }

        public async Task DeleteAllAsync(string tableName,
                                         IEnumerable<ITableEntity> entities,
                                         CancellationToken cancellationToken = default)
        {
            TableClient tableClient = GetTableClient(tableName);
            foreach (var chunk in GetActionChunks(entities, TableTransactionActionType.Delete))
                await tableClient.SubmitTransactionAsync(chunk, cancellationToken);
        }

        public async Task DeleteAllWithPartitionAsync(string tableName, 
                                                      string partitionKey)
        {
            TableClient tableClient = GetTableClient(tableName);
            var pages = tableClient.QueryAsync<TableEntity>(e => e.PartitionKey == partitionKey, maxPerPage: 100).AsPages();
            await foreach (var page in pages)
            {
                var actions = page.Values.Select(e => new TableTransactionAction(TableTransactionActionType.Delete, e)).ToList();
                await tableClient.SubmitTransactionAsync(actions);
            }
        }

        public Task DeleteAsync(string tableName,
                                string rowKey,
                                string partitionKey,
                                CancellationToken cancellationToken = default)
        {
            TableClient tableClient = GetTableClient(tableName);
            return tableClient.DeleteEntityAsync(partitionKey, rowKey, cancellationToken: cancellationToken);
        }

    }
}
