using Socially.Models;
using Socially.Utils.CodeGenerators;


const string binPath = @"bin\Debug\net7.0";
string currentPath = Environment.CurrentDirectory.Replace(binPath, string.Empty);

// GENERATORS FOR MOBILE APP
var mobileModels = AggregatedType
                        .Add<LoginModel>()
                        .Add<SignUpModel>()
                        .Add<PasswordResetModel>()
                        .Add<ForgotPasswordModel>()
                        .Add<ProfileUpdateModel>()
                        .Add<PostDisplayModel>()
                        .ToEnumerable();

await RunMobileAsync(mobileModels);



Task RunMobileAsync(IEnumerable<Type> types)
{
    string mobilePath = Path.Combine(currentPath, "..", "Socially.Mobile.Logic", "Models");
    string mappingPath = Path.Combine(mobilePath, "Mappings");
    string modelsNamespace = "Socially.MobileApp.Logic.Models";
    string mappingNamespace = $"{modelsNamespace}.Mappings";

    // CLEAR FILES
    var filesToDelete = Directory.GetFiles(mobilePath).Union(Directory.GetFiles(mappingPath)).ToArray();
    foreach (var file in filesToDelete)
        File.Delete(file);

    var classTasks = types.Select(t =>
        Task.Run(() =>
        {

            Console.WriteLine($"Generating {t.Name} for mobile");

            return File.WriteAllTextAsync(
                Path.Combine(mobilePath, $"{t.Name}.g.cs"),
                GenerateUtil.MakeObservableValidatorClass(t, modelsNamespace)
            );

        }))
        .ToArray();

    var mappingTasks = types.Select(t => Task.Run(() =>
    {
        Console.WriteLine($"Generating {t.Name} mapping for mobile");

        return File.WriteAllTextAsync(
            Path.Combine(mappingPath, $"{t.Name}MappingExtensions.g.cs"),
            GenerateUtil.MakeMappings(t, "Model", "MobileModel", mappingNamespace, modelsNamespace)
        );

    }))
    .ToArray();

    var tasks = classTasks.Union(mappingTasks);

    return Task.WhenAll(tasks);

}