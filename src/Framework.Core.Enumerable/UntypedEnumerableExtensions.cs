using System;
using System.Collections;
using System.Linq;

namespace Framework.Core;

public static class UntypedEnumerableExtensions
{
    public static Array ToArray(this IEnumerable source, Type elementType)
    {
        var sourceArr = source.OfType<object>().ToArray();

        var array = Array.CreateInstance(elementType, sourceArr.Length);

        sourceArr.CopyTo(array, 0);

        return array;
    }
}
