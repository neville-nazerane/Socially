using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Utils.CodeGenerators
{
    internal class GenerateUtil
    {


        public static string MakeObservableClass(Type type,
                                                 string destNameSpace)
        {

            var fieldStrings = type.GetProperties()
                                      .Select(p => $@"
            [ObservableProperty]
            {p.PropertyType.GetFullName()} {p.Name.LowerFirstLetter()};
")
                                      .ToArray();

            return $@"
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;

namespace {destNameSpace}
{{
    
    public partial class {type.Name} : ObservableObject
    {{
        {string.Join("", fieldStrings)}            
    }}

}}

";
        }

        public static string MakeMappings(Type srcType, 
                                          string srcLabel,
                                          string destLabel,
                                          string mappingNameSpace,
                                          string destNameSpace)
        {

            string srcName = srcType.FullName;
            string destName = $"{destNameSpace}.{srcType.Name}";

            var fields = srcType.GetProperties()
                                .Select(p => $@"
                    {p.Name} = model.{p.Name}
".Replace("\n", string.Empty)
 .Replace("\r", string.Empty)
 .Replace("\t", string.Empty)
 .Trim())
                                .ToArray();


            string methodBody = $@"=> model is null ? null : 
              new() 
              {{
                  {string.Join(",\n                  ", fields).TrimStart()}
              }};";


            return $@"
//// <GENERATED CODE> //////
namespace {mappingNameSpace}
{{

    public static class {srcType.Name}MappingExtensions 
    {{

        public static {srcName} To{srcLabel}(this {destName} model)
            {methodBody}

        public static {destName} To{destLabel}(this {srcName} model)
            {methodBody}

    }}

}}

";
        }

    }
}
