using Socially.Utils.GeneratorCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Mobile.Logic.Generator
{
    internal static class GenerateCode
    {

        internal static string MakeObservableValidatorClass(Type type,
                                         string destNameSpace)
        {

            var fieldStrings = type.GetProperties()
                                      .Select(p => $@"
            [ObservableProperty]
            {p.PropertyType.GetFullName().Replace("Socially.Models.", "Socially.Mobile.Logic.Models.")} {p.Name.LowerFirstLetter()};
")
                                      .ToArray();

            return $@"
//// <GENERATED CODE> //////
using CommunityToolkit.Mvvm.ComponentModel;
using {destNameSpace}.Mappings;
using System.ComponentModel.DataAnnotations;

namespace {destNameSpace}
{{
    
    public partial class {type.Name} : ObservableObject, IValidatable
    {{

        private readonly {type.FullName} model;

        {string.Join("", fieldStrings)}        


        public {type.Name}()
        {{
            model = new();
        }}

        public bool Validate(ICollection<ValidationResult> errors)
        {{
            this.ToModel(model);
            return Validator.TryValidateObject(model, new ValidationContext(model), errors, true);
        }}

    }}

}}

";
        }

        internal static string MakeMappings(Type srcType,
                                          string srcLabel,
                                          string destLabel,
                                          string mappingNameSpace,
                                          string destNameSpace)
        {

            string srcName = srcType.FullName;
            string destName = $"{destNameSpace}.{srcType.Name}";

            var fieldsTo = srcType.GetProperties()
                                .Select(p => $@"
                    {p.Name} = model.{p.Name}{(p.IsOfNamespace("Socially.Models") ? ".ToMobileModel()" : null)}
".ClearNewLines())
                    .ToArray();

            var fieldsFrom = srcType.GetProperties()
                    .Select(p => $@"
                    {p.Name} = model.{p.Name}{(p.IsOfNamespace("Socially.Models") ? ".ToModel()" : null)}
".ClearNewLines())
        .ToArray();


            var cloneFields = srcType.GetProperties()
                                .Select(p => $@"
                    dest.{p.Name} = model.{p.Name};
".ClearNewLines())
                    .ToArray();

            var updateFieldsTo = srcType.GetProperties()
                                .Select(p => $@"
                    dest.{p.Name} = model.{p.Name}{(p.IsOfNamespace("Socially.Models") ? ".ToMobileModel()" : null)};
".ClearNewLines())
                    .ToArray();

            var updateFieldsFrom = srcType.GetProperties()
                    .Select(p => $@"
                    dest.{p.Name} = model.{p.Name}{(p.IsOfNamespace("Socially.Models") ? ".ToModel()" : null)};
".ClearNewLines())
        .ToArray();



            string methodBodyTo = $@"=> model is null ? null : 
              new() 
              {{
                  {string.Join(",\n                  ", fieldsTo).TrimStart()}
              }};";

            string methodBodyFrom = $@"=> model is null ? null : 
              new() 
              {{
                  {string.Join(",\n                  ", fieldsFrom).TrimStart()}
              }};";

            string updateMethodBodyTo = $@"
        {{
            {string.Join("\n            ", updateFieldsTo).TrimStart()}
            return dest;
        }}".TrimStart();

            string updateMethodBodyFrom = $@"
        {{
            {string.Join("\n            ", updateFieldsFrom).TrimStart()}
            return dest;
        }}".TrimStart();

            string cloneMethodBody = $@"
        {{
            {string.Join("\n            ", cloneFields).TrimStart()}
            return dest;
        }}".TrimStart();


            return $@"
//// <GENERATED CODE> //////

namespace {mappingNameSpace}
{{

    public static class {srcType.Name}MappingExtensions 
    {{

        public static async Task<IEnumerable<{srcName}>> To{srcLabel}(this Task<IEnumerable<{destName}>> modelTask)
            => (await modelTask).To{srcLabel}();

        public static IEnumerable<{srcName}> To{srcLabel}(this IEnumerable<{destName}> model)
            => model == null ? null : model.Select(m => m.To{srcLabel}()).ToList();

        public static async Task<List<{srcName}>> To{srcLabel}(this Task<ICollection<{destName}>> modelTask)
            => (await modelTask).To{srcLabel}();

        public static List<{srcName}> To{srcLabel}(this ICollection<{destName}> model)
            => model == null ? null : model.Select(m => m.To{srcLabel}()).ToList();

        public static async Task<{srcName}> To{srcLabel}(this Task<{destName}> modelTask)
            => (await modelTask).To{srcLabel}();

        public static {srcName} To{srcLabel}(this {destName} model)
            {methodBodyFrom}

        public static {srcName} To{srcLabel}(this {destName} model, {srcName} dest)
        {updateMethodBodyFrom}

        public static {destName} CloneFrom(this {destName} dest, {destName} model)
        {cloneMethodBody}

        public static async Task<IEnumerable<{destName}>> To{destLabel}(this Task<IEnumerable<{srcName}>> modelTask)
            => (await modelTask).To{destLabel}();

        public static IEnumerable<{destName}> To{destLabel}(this IEnumerable<{srcName}> model)
            => model == null ? null : model.Select(m => m.To{destLabel}()).ToList();   

        public static async Task<List<{destName}>> To{destLabel}(this Task<ICollection<{srcName}>> modelTask)
            => (await modelTask).To{destLabel}();

        public static List<{destName}> To{destLabel}(this ICollection<{srcName}> model)
            => model == null ? null : model.Select(m => m.To{destLabel}()).ToList();   

        public static async Task<{destName}> To{destLabel}(this Task<{srcName}> modelTask)
            => (await modelTask).To{destLabel}();

        public static {destName} To{destLabel}(this {srcName} model)
            {methodBodyTo}

        public static {destName} To{destLabel}(this {srcName} model, {destName} dest)
        {updateMethodBodyTo}

    }}

}}

";
        }


    }
}
