using CommonFramework;

using Framework.Core;
using Framework.Exceptions;

namespace Framework.Persistent;

public static class HierarchicalPersistentDomainObjectBaseExtensions
{
    public static IEnumerable<T> GetAllParents<T>(this T source, bool skipFirstElement = false)
        where T : class, IParentSource<T>
    {
        return source.GetAllElements(v => v.Parent, skipFirstElement);
    }


    public static bool HasCyclic<T>(this T obj)
            where T : class, IParentSource<T>
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        return obj.GetAllParents(true).Contains(obj);
    }

    public static void CheckCyclicReference<T>(this T obj)
            where T : class, IParentSource<T>, IVisualIdentityObject
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        obj.CheckCyclicReference(obj.Name);
    }

    public static void CheckCyclicReference<T>(this T obj, string objName)
            where T : class, IParentSource<T>
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        if (obj.HasCyclic())
        {
            objName.RaiseCyclicException<T>();
        }
    }

    private static void RaiseCyclicException<T>(this string objName)
    {
        throw new BusinessLogicException($"Cyclic reference for {typeof(T).Name}{objName.Maybe(v => " (" + v + ")")} detected");
    }

}
