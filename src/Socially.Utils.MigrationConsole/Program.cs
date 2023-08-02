using Microsoft.EntityFrameworkCore;
using Socially.Server.DataAccess;


switch (args.Length)
{
    case 0:
        Console.Error.WriteLine("Pass connection string as first arg");
        return;
    case 1:
        Console.Error.WriteLine("Pass realtime conn string as second arg");
        break;
}



await MigrateDatabaseAsync<ApplicationDbContext>(args[0]);
await MigrateDatabaseAsync<RealTimeDbContext>(args[1]);










static async Task MigrateDatabaseAsync<T>(string connectionString) where T : DbContext
{
    var builder = new DbContextOptionsBuilder<T>().UseSqlServer(connectionString);
    await using var dbContext = (T)Activator.CreateInstance(typeof(T), builder.Options);
    await dbContext.Database.MigrateAsync();
}
