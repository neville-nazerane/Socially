using Socially.Mobile.Logic.Generator;
using Socially.Models;
using Socially.Utils.CodeGenerators;
using System.Reflection;

const string binPath = @"bin\Debug\net7.0";
string currentPath = args.FirstOrDefault() ?? Environment.CurrentDirectory.Replace(binPath, string.Empty);
string mobileLogicPath = Path.Combine(currentPath, "..", "Socially.Mobile.Logic");
string mobilePath = Path.Combine(currentPath, "..", "Socially.MobileApp");


// GENERATORS FOR MOBILE APP
var mobileModels = AggregatedType
                        .Add<LoginModel>()
                        .Add<SignUpModel>()
                        .Add<PasswordResetModel>()
                        .Add<ForgotPasswordModel>()
                        .Add<ProfileUpdateModel>()
                        .Add<UserSummaryModel>()
                        .Add<PostDisplayModel>()
                        .Add<DisplayCommentModel>()
                        .Add<AddPostModel>()
                        .Add<AddCommentModel>()
                        .ToEnumerable();

await SetMobileModelsAsync(mobileModels);


Task SetMobileModelsAsync(IEnumerable<Type> types)
{
    string mobilePath = Path.Combine(currentPath, "..", "Socially.Mobile.Logic", "Generated", "Models");
    string mappingPath = Path.Combine(mobilePath, "Mappings");
    string modelsNamespace = "Socially.Mobile.Logic.Models";
    string mappingNamespace = $"{modelsNamespace}.Mappings";

    if (!Directory.Exists(mappingPath))
        Directory.CreateDirectory(mobilePath);

    if (!Directory.Exists(mappingPath))
        Directory.CreateDirectory(mappingPath);

    // CLEAR FILES
    var filesToDelete = Directory.GetFiles(mobilePath)
                                 .Union(Directory.GetFiles(mappingPath))
                                 .Where(f => f.EndsWith(".g.cs"))
                                 .ToArray();
    foreach (var file in filesToDelete)
        File.Delete(file);

    var classTasks = types.Select(t =>
        Task.Run(() =>
        {
            Console.WriteLine($"Generating {t.Name} for mobile");

            return File.WriteAllTextAsync(
                Path.Combine(mobilePath, $"{t.Name}.g.cs"),
                GenerateCode.MakeObservableValidatorClass(t, modelsNamespace)
            );

        }))
        .ToArray();

    var mappingTasks = types.Select(t => Task.Run(() =>
    {
        Console.WriteLine($"Generating {t.Name} mapping for mobile");

        return File.WriteAllTextAsync(
            Path.Combine(mappingPath, $"{t.Name}MappingExtensions.g.cs"),
            GenerateCode.MakeMappings(t, "Model", "MobileModel", mappingNamespace, modelsNamespace)
        );

    }))
    .ToArray();

    var tasks = classTasks.Union(mappingTasks);

    return Task.WhenAll(tasks);

}

