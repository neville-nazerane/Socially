using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Socially.Server.DataAccess;


string realtimeConnStr = null;

switch (args.Length)
{
    case 0:
        Console.Error.WriteLine("Pass connection string as first arg");
        return;
    case 1:
        Console.WriteLine("No realtime connection string passed. Using first connection string");
        realtimeConnStr = ChangeDatabaseInConnectionString(args[0], "realtime");
        break;
    case 2:
        realtimeConnStr = args[1];
        break;
}



await MigrateDatabaseAsync<ApplicationDbContext>(args[0]);
await MigrateDatabaseAsync<RealTimeDbContext>(realtimeConnStr);







static async Task MigrateDatabaseAsync<T>(string connectionString) where T : DbContext
{
    var builder = new DbContextOptionsBuilder<T>().UseSqlServer(connectionString);
    await using var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options);
    await dbContext.Database.MigrateAsync();
}

static string ChangeDatabaseInConnectionString(string originalConnectionString, string newDatabaseName)
{
    SqlConnectionStringBuilder builder = new(originalConnectionString);

    // Change the database in the connection string
    builder.InitialCatalog = newDatabaseName;

    return builder.ConnectionString;
}
