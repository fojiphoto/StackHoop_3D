using System.Collections.Generic;

public static class IListExtensions
{
    public static bool InBounds<T>(this IList<T> list, int i)
    {
        return i > -1 && i < list.Count;
    }
}