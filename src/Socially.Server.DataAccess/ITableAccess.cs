using Azure.Data.Tables;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.DataAccess
{
    public interface ITableAccess
    {
        Task AddAsync(string tableName, ITableEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(string tableName, IEnumerable<ITableEntity> entities, CancellationToken cancellationToken = default);
        Task CreateTableIfNotExistAsync(string tableName, CancellationToken cancellationToken = default);
        Task DeleteAllAsync(string tableName, IEnumerable<ITableEntity> entities, CancellationToken cancellationToken = default);
        Task DeleteAllWithPartitionAsync(string tableName, string partitionKey);
        Task DeleteAsync(string tableName, string rowKey, string partitionKey, CancellationToken cancellationToken = default);
        IAsyncEnumerable<T> ListByPartitionAsync<T>(string tableName, string partitionKey, int pageSize) where T : class, ITableEntity, new();
        IAsyncEnumerable<TableEntity> ListByPartitionAsync(string tableName, string partitionKey, int pageSize);
    }
}