using Microsoft.Extensions.Configuration;
using Socially.MobileApp.Generator;
using Socially.Utils.GeneratorCommon;
using System.Reflection;
using System.Text;

const string binPath = @"bin\Debug\net7.0";
string currentPath = args.FirstOrDefault() ?? Environment.CurrentDirectory.Replace(binPath, string.Empty);
string mobileLogicPath = Path.Combine(currentPath, "..", "Socially.Mobile.Logic");
string mobilePath = Path.Combine(currentPath, "..", "Socially.MobileApp");


// GENERATE MAUI PAGE DEFAULTS
await SetMauiPageDefaultsAsync();


Task SetMobileConfigAsync(IConfiguration configuration)
{
    string mainMobilePath = Path.Combine(currentPath, "..", "Socially.MobileApp");
    var configFile = Path.Combine(mainMobilePath, "Utils", "Configs.local.cs");

    var properties = configuration.GetChildren()
                                     .Select(c => $"            {c.Key.UpperFirstLetter()} = \"{c.Value}\";")
                                     .ToArray();

    return File.WriteAllTextAsync(configFile, $@"

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


// CONFIGURATIONS FROM SECRET MANAGER
var configs = new ConfigurationBuilder()
                        .AddUserSecrets("Socially Client")
                        .Build();

await SetMobileConfigAsync(configs);
await SetWebsiteConfigsAsync(configs);


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

async Task SetMauiPageDefaultsAsync()
{
    var genPath = FileUtils.GrabPath(mobilePath, "Generated");
    var genPagePath = FileUtils.GrabPathAndClear(genPath, "Pages");
    var genComponentsPath = FileUtils.GrabPathAndClear(genPath, "Components");
    var pagePath = FileUtils.GrabPath(mobilePath, "Pages");
    var componentsPath = FileUtils.GrabPath(mobilePath, "Components");
    var vmPath = FileUtils.GrabPath(mobileLogicPath, "ViewModels");

    var pages = Directory.GetFiles(pagePath)
                         .Where(p => p.EndsWith("Page.xaml.cs"))
                         .Select(p => new FileInfo(p).Name[..^"Page.xaml.cs".Length])
                         .ToArray();

    var components = Directory.GetFiles(componentsPath)
                     .Where(p => p.EndsWith("xaml.cs"))
                     .Select(p => new FileInfo(p).Name[..^".xaml.cs".Length])
                     .ToArray();

    var viewModels = Assembly.Load("Socially.Mobile.Logic")
                             .GetTypes()
                             .Where(t => t.Name.EndsWith("ViewModel"))
                             .ToArray();

    var componentModels = Assembly.Load("Socially.Mobile.Logic")
                             .GetTypes()
                             .Where(t => t.Name.EndsWith("ComponentModel"))
                             .ToArray();


    // delete all from page

    var injectablePages = new List<string>();
    var injectableComponents = new List<string>();

    foreach (var vm in viewModels)
    {
        var page = pages.SingleOrDefault(p => p == vm.Name[..^"ViewModel".Length]);
        if (page is not null)
        {
            injectablePages.Add(page);
            Console.WriteLine("Generating for page: " + page);
            var pageContent = GenerateCode.MakePageClass(vm,
                                                         typeof(Socially.Mobile.Logic.ViewModels.ViewModelBase),
                                                         "Socially.Mobile.Logic.ViewModels",
                                                         "Socially.MobileApp.Pages",
                                                         page);
            var filePath = Path.Combine(genPagePath, $"{page}Page.g.cs");

            await File.WriteAllTextAsync(filePath, pageContent);
        }
    }

    foreach (var cm in componentModels)
    {
        var comp = components.SingleOrDefault(c => c == cm.Name[..^"ComponentModel".Length]);
        if (comp is not null)
        {
            injectableComponents.Add(comp);
            Console.WriteLine("Generating for Component: " + comp);
            var filePath = Path.Combine(genComponentsPath, $"{comp}.g.cs");
            var classContent = GenerateCode.MakeComponentClass(comp);
            await File.WriteAllTextAsync(filePath, classContent);
        }
    }

    Console.WriteLine("Creating MAUI Program");

    var mainCode = GenerateCode.MakeMauiPartialProgram(injectablePages, injectableComponents);
    var mainPath = Path.Combine(genPath, "MauiProgram.g.cs");
    await File.WriteAllTextAsync(mainPath, mainCode);

}


