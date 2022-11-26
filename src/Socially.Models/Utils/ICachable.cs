using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Socially.Models.Utils
{
    public interface ICachable<TKey, TData> 
        where TData : ICachable<TKey, TData>, new()
    {

        void CopyFrom(TData data);

        TKey GetCacheKey();

    }
}
