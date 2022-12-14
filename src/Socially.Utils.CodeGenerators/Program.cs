using Microsoft.Extensions.Configuration;
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
                        .Add<UserSummaryModel>()
                        .Add<PostDisplayModel>()
                        .ToEnumerable();

await RunMobileAsync(mobileModels);

var configs = new ConfigurationBuilder()
                        .AddUserSecrets("Socially Client")
                        .Build();

await SetMobileConfigAsync(configs);
await SetWebsiteConfigsAsync(configs);


Task SetMobileConfigAsync(IConfiguration configuration)
{
    string mainMobilePath = Path.Combine(currentPath, "..", "Socially.MobileApp");
    var configFile = Path.Combine(mainMobilePath, "Utils", "Configs.local.cs");

    var properties =  configuration.GetChildren()
                                     .Select(c => $"            {c.Key.UpperFirstLetter()} = \"{c.Value}\";")
                                     .ToArray();

    return File.WriteAllTextAsync(configFile,$@"

namespace Socially.MobileApp.Utils
{{
    public partial class Configs
    {{

        static Configs()
        {{
{string.Join("\n", properties)}
        }}

    }}
}}

    ");
}

Task SetWebsiteConfigsAsync(IConfiguration configuration)
{
    string websitePath = Path.Combine(currentPath, "..", "Socially.Website");
    var configFile = Path.Combine(websitePath, "wwwroot", "appsettings.Development.json");


    var properties = configuration.GetChildren()
                                  .Select(c => $"   \"{c.Key}\": \"{c.Value}\"")
                                  .ToArray();

    return File.WriteAllTextAsync(configFile, $@"
    
{{
{string.Join(",\n", properties)}
}}

");

}

Task RunMobileAsync(IEnumerable<Type> types)
{
    string mobilePath = Path.Combine(currentPath, "..", "Socially.Mobile.Logic", "Models");
    string mappingPath = Path.Combine(mobilePath, "Mappings");
    string modelsNamespace = "Socially.Mobile.Logic.Models";
    string mappingNamespace = $"{modelsNamespace}.Mappings";

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