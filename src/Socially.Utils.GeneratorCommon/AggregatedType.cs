using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Utils.CodeGenerators
{
    public class AggregatedType
    {

        private readonly IEnumerable<Type> _types;

        private AggregatedType(IEnumerable<Type> types)
        {
            _types = types;
        }

        public static AddingTypes Add<T>() => new(new(new[] { typeof(T) }));


        public class AddingTypes
        {
            private readonly AggregatedType source;

            public AddingTypes(AggregatedType source)
            {
                this.source = source;
            }

            public AddingTypes Add<T>()
            {
                var newTypes = source._types.Union(new[] { typeof(T) }).ToArray();
                return new(new(newTypes));
            }

            public IEnumerable<Type> ToEnumerable() => source._types;

        }

    }
}
