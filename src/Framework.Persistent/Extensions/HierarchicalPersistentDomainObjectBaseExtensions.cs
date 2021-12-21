using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Exceptions;
using Framework.HierarchicalExpand;

namespace Framework.Persistent
{
    public static class HierarchicalPersistentDomainObjectBaseExtensions
    {
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

        public static bool HasChildCyclic<T>(this T obj)
            where T : class, IChildrenSource<T>
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.GetGraphElements(l => l.Children, true).Contains(obj);
        }

        public static void CheckChildCyclicReference<T>(this T obj)
            where T : class, IChildrenSource<T>, IVisualIdentityObject
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            obj.CheckChildCyclicReference(obj.Name);
        }

        public static void CheckChildCyclicReference<T>(this T obj, string objName)
            where T : class, IChildrenSource<T>
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (obj.HasChildCyclic())
            {
                objName.RaiseCyclicException<T>();
            }
        }

        private static void RaiseCyclicException<T>(this string objName)
        {
            throw new BusinessLogicException($"Cyclic reference for {typeof(T).Name}{objName.Maybe(v => " (" + v + ")")} detected");
        }

        /// <summary>
        /// Finding the Lowest Common Ancestor
        /// </summary>
        public static T LowestCommonAncestor<T>(this IEnumerable<T> objs)
            where T : class, IParentSource<T>

        {
            if (objs == null) throw new ArgumentNullException(nameof(objs));

            if (!objs.Any())
            {
                return null;
            }

            var list = objs.ToList();
            var parents = new List<List<T>>();
            var minLen = int.MaxValue;

            list.Foreach(x =>
            {
                var currentParents = x.GetAllParents().ToList();
                parents.Add(currentParents);
                minLen = Math.Min(minLen, currentParents.Count);
            });

            for (int i = 0; i < parents.Count; i++)
            {
                parents[i] = parents[i].Skip(parents[i].Count - minLen).ToList();
            }

            for (int i = 0; i < parents[0].Count; i++)
            {
                var equal = true;
                for (int j = 0; j < parents.Count; j++)
                {
                    if (parents[j][i] != parents[0][i])
                    {
                        equal = false;
                        break;
                    }
                }
                if (equal)
                {
                    return parents[0][i];
                }
            }

            return null;
        }
    }
}
