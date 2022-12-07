using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Utils.CodeGenerators
{
    internal static class StringExtensions
    {

        internal static string LowerFirstLetter(this string str)
            => str[0].ToString().ToLower() + str[1..];

        internal static string GetFullName(this Type type)
        {
            string fullName = type.FullName; // $"{type.Namespace}.{type.Name.Substring(0, type.Name.IndexOf("`"))}"; 
            if (type.IsGenericType)
            {
                fullName = fullName[..fullName.IndexOf("`")];
                var subTypes = type.GetGenericArguments()
                                   .Select(t => t.GetFullName())
                                   .ToArray();
                var gentypes = string.Join(", ", subTypes);
                fullName = $"{fullName}<{gentypes}>";
            }
            return fullName;
        }

    }
}
