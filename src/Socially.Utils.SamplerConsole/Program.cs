//using MongoDB.Bson.Serialization;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using Microsoft.Extensions.Configuration;
//using MongoDB.Driver.Linq;

//var config = new ConfigurationBuilder().AddUserSecrets("Socially.WebAPI").Build();


//var client = new MongoClient(config["mongoConnString"]);

//var db = client.GetDatabase("dev-cosmos");


////await db.GetCollection<Post>("posts").InsertOneAsync(new Post
////{
////    Id = 80,
////    Text = "eighty a new hope",
////    Comments = new Comment[]
////    {
////        new Comment
////        {
////            Id = Guid.NewGuid(),
////            Text = "abc"
////        },
////        new Comment
////        {
////            Id = new Guid("bddc23d0-2bff-4994-b178-2a052ba624c9"),
////            Text = "second"
////        },
////        new Comment
////        {
////            Id = Guid.NewGuid(),
////            Text = "third"
////        }
////    }
////});


//var res = await db.GetCollection<Post>("posts").FindAsync(d => d.Id == 80);

//var post = await res.FirstOrDefaultAsync();
//var comment = post?.Comments?.SingleOrDefault(c => c.Id == 10);

//var start = DateTimeOffset.UtcNow;

//var filter = Builders<Post>.Filter.And(
//    Builders<Post>.Filter.Eq(p => p.Id, 80),
//    Builders<Post>.Filter.Eq("Comments._id", new Guid("cae2a14b-7302-42fc-b837-62f97bcdc261"))
//    //Builders<Post>.Filter.Eq("Comments.Text", "second")
//);


////var comment = await db.GetCollection<Post>("posts")
////                            .Find(filter).Project(p => p.Comments)
////                            .ToListAsync();


////var res2 = await db.GetCollection<Post>("posts")
////                            .FindAsync(d => d.Id == 40);




////await db.GetCollection<Post>("posts")
////                .Find(p => p.Id == 70)
////                .Project(p => p.Comments.FirstN(c => c.Id, new Guid("bddc23d0-2bff-4994-b178-2a052ba624c9")))


////await db.GetCollection<Post>("posts")
////                .InsertOneAsync(new Post
////                {
////                    Id = 70,
////                    Text = "a whole new sample!!",
////                    CreatorId = 40
////                });

//var insertStart = DateTime.UtcNow;

//var item = new Comment
//{
//    Id = 10,
//    Text = "inner seventh"
//};

//var update = Builders<Post>.Update.Push("Comments.$.Comments", item);
////await db.GetCollection<Post>("posts")
////                .FindOneAndUpdateAsync(filter, update);

//var project = Builders<Post>.Projection.Include("Comments.$").Exclude("_id");
//var com = await db.GetCollection<Post>("posts").Find(filter)
//                                             .Project(project)
//                                             .SingleOrDefaultAsync();


////await db.GetCollection<Post>("posts")
////              .UpdateOneAsync(p => p.Id == 70, Builders<Post>.Update.AddToSet(p => p.Comments.Last().Comments, item));

////var posts = new List<Post>();
////for (int i = 0; i < 40; i++)
////{
////    posts.Add(new Post
////    {
////        Id = 10 + i,
////        CreatorId = i % 8,
////        Text = $"adding stuff {i} in data"
////    });
////}

////await db.GetCollection<Post>("posts").InsertManyAsync(posts);
//var ru = await db.GetRUAsync();
//Console.WriteLine($"Added posts in {(DateTime.UtcNow - insertStart).TotalSeconds} sec and {ru} RUs");

//Console.ReadLine();


//class GetLastRequestStatisticsCommand : Command<Dictionary<string, object>>
//{
//    public override RenderedCommand<Dictionary<string, object>> Render(IBsonSerializerRegistry serializerRegistry)
//    {
//        return new RenderedCommand<Dictionary<string, object>>(new BsonDocument("getLastRequestStatistics", 1), serializerRegistry.GetSerializer<Dictionary<string, object>>());
//    }
//}

//static class Utilities
//{
//    public static async Task<double> GetRUAsync(this IMongoDatabase db)
//    {
//        var stats = await db.RunCommandAsync(new GetLastRequestStatisticsCommand());
//        return (double)stats["RequestCharge"];
//    }
//}