using Microsoft.EntityFrameworkCore;
using Socially.Server.DataAccess;

if (args.Length == 0)
{
    Console.Error.WriteLine("Pass connection string as first arg");
    return;
}

var builder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(args[0]);
var dbContext = new ApplicationDbContext(builder.Options);
await dbContext.Database.MigrateAsync();