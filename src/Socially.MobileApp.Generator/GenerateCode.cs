using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.MobileApp.Generator
{
    public static class GenerateCode
    {

        public static string MakePageClass(Type viewModel,
                                           Type baseType,
                                           string vmNamespace,
                                           string pageNamespace,
                                           string pageName)
        {


            //if (viewModel.IsAssignableTo(baseType))
            //{

            //}


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
