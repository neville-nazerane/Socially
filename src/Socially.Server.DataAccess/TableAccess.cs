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
    public class TableAccess
    {

        private TableServiceClient _tableServiceClient;
        private ConcurrentDictionary<string, TableClient> _clients;

        public TableAccess(string connString)
        {
            _tableServiceClient = new TableServiceClient(connString);
            _clients = new();
        }

        TableClient GetTableClient(string tableName) 
            => _clients.GetOrAdd(tableName, t => _tableServiceClient.GetTableClient(tableName));

        public Task AddAsync(string tableName, ITableEntity entity, CancellationToken cancellationToken = default)
        {
            TableClient tableClient = GetTableClient(tableName);
            return tableClient.AddEntityAsync(entity, cancellationToken);
        }

        public IAsyncEnumerable<T> ListByPartitionAsync<T>(string tableName, 
                                                           string partitionKey,
                                                           int pageSize) 
            where T : class, ITableEntity, new()
        {
            TableClient tableClient = GetTableClient(tableName);
            var entities = tableClient.QueryAsync<T>(e => e.PartitionKey == partitionKey, maxPerPage: pageSize);
            return entities;
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
