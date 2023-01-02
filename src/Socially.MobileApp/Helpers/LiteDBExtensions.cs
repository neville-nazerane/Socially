using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDB
{
    public static class LiteDBExtensions
    {

        public static BsonValue ToBsonValue(this object obj)
            => obj switch
                {
                    int v => v,
                    double v => v,
                    long v => v,
                    decimal v => v,
                    ulong v => v,
                    string v => v,
                    byte[] v => v,
                    Guid v => v,
                    bool v => v,
                    DateTime v => v,
                    _ => throw new InvalidCastException($"Can't cast {obj.GetType()} to BsonValue"),
                };

    }
}
