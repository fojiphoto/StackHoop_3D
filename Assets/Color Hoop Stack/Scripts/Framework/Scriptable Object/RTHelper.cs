using System.Collections.Generic;
using System.Linq;

public static class RTHelper
{
    /// <summary>
    /// Creates a dictionary out of the passed keys and values
    /// </summary>
    public static Dictionary<T, U> CreateDictionary<T, U>(IEnumerable<T> keys, IList<U> values)
    {
        return keys.Select((k, i) => new { k, v = values[i] }).ToDictionary(x => x.k, x => x.v);
    }
}