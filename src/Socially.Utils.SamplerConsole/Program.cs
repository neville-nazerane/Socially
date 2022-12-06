using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Socially.Server.DataAccess;
using Socially.Server.ModelMappings;

var config = new ConfigurationBuilder().AddUserSecrets("Socially.WebAPI").Build();


var builder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(config.GetConnectionString("db"));
var dbContext = new ApplicationDbContext(builder.Options);

//string query = "hello";
//int userId = 1;

//var sql = dbContext.Users.Where(u => u.FirstName.Contains(query)
//                                   || u.LastName.Contains(query))
//                                 .Where(u => !dbContext.Users.Single(u => u.Id == userId).Friends.Any(f => f.FriendUserId == u.Id))
//                                 .SelectAsSummaryModel()
//                                 .ToQueryString();


//Console.WriteLine("Hi world");