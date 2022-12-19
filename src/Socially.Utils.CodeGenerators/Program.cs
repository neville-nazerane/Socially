using Microsoft.Extensions.Configuration;
using Socially.Models;
using Socially.Utils.CodeGenerators;
using System.Reflection;

const string binPath = @"bin\Debug\net7.0";
string currentPath = Environment.CurrentDirectory.Replace(binPath, string.Empty);
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
                        .ToEnumerable();

await SetMobileModelsAsync(mobileModels);

// CONFIGURATIONS FROM SECRET MANAGER
var configs = new ConfigurationBuilder()
                        .AddUserSecrets("Socially Client")
                        .Build();

await SetMobileConfigAsync(configs);
await SetWebsiteConfigsAsync(configs);

// GENERATE MAUI PAGE DEFAULTS
await SetMauiPageDefaultsAsync();



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

async Task SetMauiPageDefaultsAsync()
{
    var genPath = GrabPath(mobilePath, "Generated");
    var genPagePath = GrabPathAndClear(genPath, "Pages");
    var pagePath = GrabPath(mobilePath, "Pages");
    var vmPath = GrabPath(mobileLogicPath, "ViewModels");

    var pages = Directory.GetFiles(pagePath)
                         .Where(p => p.EndsWith("Page.xaml.cs"))
                         .Select(p => new FileInfo(p).Name[..^"Page.xaml.cs".Length])
                         .ToArray();

    var viewModels = Assembly.Load("Socially.Mobile.Logic")
                             .GetTypes()
                             .Where(t => t.Name.EndsWith("ViewModel"))
                             .ToArray();

    // delete all from page

    foreach (var vm in viewModels)
    {
        var page = pages.SingleOrDefault(p => p == vm.Name[..^"ViewModel".Length]);
        if (page is not null)
        {
            Console.WriteLine("Generating for page: " + page);
            var pageContent = GenerateUtil.MakePageClass(vm,
                                                         typeof(Socially.Mobile.Logic.ViewModels.ViewModelBase),
                                                         "Socially.Mobile.Logic.ViewModels",
                                                         "Socially.MobileApp.Pages",
                                                         page);
            var filePath = Path.Combine(genPagePath, $"{page}Page.g.cs");

            await File.WriteAllTextAsync(filePath, pageContent);

            var pageFile = Path.Combine(pagePath, $"{page}Page.xaml.cs");
            var deadCtor = @"public LoginPage()
	{
		InitializeComponent();
    }";
            var oldContent = await File.ReadAllTextAsync(pageFile);
            await File.WriteAllTextAsync(pageFile, oldContent.Replace(deadCtor, string.Empty));

        }
    }
}

string GrabPath(params string[] parts)
{
    var path = Path.Combine(parts);
    if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

    return path;
}

string GrabPathAndClear(params string[] parts)
{
    var path = GrabPath(parts);
    foreach (var file in Directory.GetFiles(path))
        File.Delete(file);

    return path;
}