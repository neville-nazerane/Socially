using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Generator
{
    public static class GenerateCode
    {

        public static string MakeMauiPartialProgram(IEnumerable<string> pages)
        {

            var pageInjections = pages.Select(p => $".AddTransient<{p}Page>().AddTransient<{p}ViewModel>()")
                                      .ToArray();

            var str = $@"

using Socially.Mobile.Logic.ViewModels;
using Socially.MobileApp.Pages;

namespace Socially.MobileApp;

public static partial class MauiProgram
{{

    static partial void AppPageInjections(IServiceCollection services)
    {{
        services{string.Join("\n", pageInjections)};
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
                    
        public {pageName}Page({pageName}ViewModel viewModel)
	    {{
		    InitializeComponent();
            BindingContext = viewModel;
        }}

    }}

}}


";

            return classContent;
        }


    }
}
