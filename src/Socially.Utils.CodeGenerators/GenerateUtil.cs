﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Utils.CodeGenerators
{
    internal class GenerateUtil
    {


        public static string MakeObservableValidatorClass(Type type,
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
using {destNameSpace}.Mappings;
using System.ComponentModel.DataAnnotations;

namespace {destNameSpace}
{{
    
    public partial class {type.Name} : ObservableValidator
    {{

        private readonly ValidationContext validationContext;
        private readonly {type.FullName} model;

        {string.Join("", fieldStrings)}        


        public {type.Name}()
        {{
            model = new();
            validationContext = new ValidationContext(model);
        }}

        public bool Validate(ICollection<ValidationResult> errors)
        {{
            this.ToModel(model);
            return Validator.TryValidateObject(model, new ValidationContext(model), errors);
        }}

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
".ClearNewLines())
                    .ToArray();

            var updateFields = srcType.GetProperties()
                                .Select(p => $@"
                    dest.{p.Name} = model.{p.Name};
".ClearNewLines())
                    .ToArray();


            string methodBody = $@"=> model is null ? null : 
              new() 
              {{
                  {string.Join(",\n                  ", fields).TrimStart()}
              }};";

            string updateMethodBody = $@"
        {{
            {string.Join("\n            ", updateFields).TrimStart()}
            return dest;
        }}".TrimStart();


            return $@"
//// <GENERATED CODE> //////
namespace {mappingNameSpace}
{{

    public static class {srcType.Name}MappingExtensions 
    {{

        public static {srcName} To{srcLabel}(this {destName} model)
            {methodBody}

        public static {srcName} To{srcLabel}(this {destName} model, {srcName} dest)
        {updateMethodBody}

        public static {destName} To{destLabel}(this {srcName} model)
            {methodBody}

        public static {destName} To{destLabel}(this {srcName} model, {destName} dest)
        {updateMethodBody}

    }}

}}

";
        }

    }
}
