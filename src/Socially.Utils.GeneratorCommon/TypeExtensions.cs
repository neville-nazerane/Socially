using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Utils.GeneratorCommon
{
    public static class TypeExtensions
    {

        public static bool IsOfNamespace(this PropertyInfo info, string name)
            => info.PropertyType.IsOfNamespace(name);

        public static bool IsOfNamespace(this Type type, string name) 
            => type.Namespace == name
                    || type.GenericTypeArguments.Any(t => t.Namespace == name);

    }
}
