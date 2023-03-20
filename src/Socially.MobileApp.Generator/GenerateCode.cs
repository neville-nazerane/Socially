using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Generator
{
    public static class GenerateCode
    {

        public static string MakeMauiPartialProgram(IEnumerable<string> pages, IEnumerable<string> components)
        {

            var injections = pages.Select(p => $".AddTransient<{p}Page>().AddTransient<{p}ViewModel>()")
                                  .Union(components.Select(c => $".AddTransient<{c}>().AddTransient<{c}ComponentModel>()"))
                                      .ToArray();

            var str = $@"

using Socially.Mobile.Logic.ComponentModels;
using Socially.Mobile.Logic.ViewModels;
using Socially.MobileApp.Components;
using Socially.MobileApp.Pages;

namespace Socially.MobileApp;

public static partial class MauiProgram
{{

    static partial void AppPageInjections(IServiceCollection services)
    {{
        services{string.Join("\n", injections)};
    }}
        
}}

";

            return str;

        }

        public static string MakePageClass(Type viewModel,
                                           Type baseType,
                                           string vmNamespace,
                                           string pageNamespace,
                                           string pageName)
        {


            string classContent = @$"

using {vmNamespace};

namespace {pageNamespace} 
{{

    public partial class {pageName}Page
    {{

        public {pageName}ViewModel ViewModel {{ get; }}
                    
        public {pageName}Page({pageName}ViewModel viewModel)
	    {{
		    InitializeComponent();
            BindingContext = viewModel;
            ViewModel = viewModel;
        }}

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {{
            await ViewModel.OnNavigatedAsync();
            base.OnNavigatedTo(args);
        }}

        protected override async void OnNavigatedFrom(NavigatedFromEventArgs args)
        {{
            await ViewModel.OnNavigatedFromAsync();
            base.OnNavigatedFrom(args);
        }}

    }}


}}


";

            return classContent;
        }

        public static string MakeComponentClass(string componentName)
        {

            var classContent = $@"
using Socially.Mobile.Logic.ComponentModels;
using Socially.MobileApp.Utils;

namespace Socially.MobileApp.Components;

public partial class {componentName} 
{{

    private {componentName}ComponentModel _componentModel;
    public {componentName}ComponentModel ComponentModel => _componentModel ??= ({componentName}ComponentModel)(BindingContext = ServicesUtil.Get<{componentName}ComponentModel>());

}}

";
            return classContent;

        }


    }
}
