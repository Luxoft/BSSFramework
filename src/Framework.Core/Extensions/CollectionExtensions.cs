using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class CollectionExtensions
    {
        public static ICollection<T> WithPreAddAction<T>([NotNull] this ICollection<T> baseCollection, [NotNull] Action<T> action)
        {
            if (baseCollection == null) throw new ArgumentNullException(nameof(baseCollection));
            if (action == null) throw new ArgumentNullException(nameof(action));

            return new CustomActionCollection<T>(baseCollection, action, null, null, null);
        }

        public static ICollection<T> WithPostAddAction<T>(this ICollection<T> baseCollection, [NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return new CustomActionCollection<T>(baseCollection, null, action, null, null);
        }

        public static ICollection<T> WithPreRemoveAction<T>(this ICollection<T> baseCollection, [NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return new CustomActionCollection<T>(baseCollection, null, null, action, null);
        }

        public static ICollection<T> WithPostRemoveAction<T>(this ICollection<T> baseCollection, [NotNull] Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return new CustomActionCollection<T>(baseCollection, null, null, null, (item, _) => action(item));
        }

        public static ICollection<T> WithPostRemoveAction<T>(this ICollection<T> baseCollection, [NotNull] Action<T, bool> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            return new CustomActionCollection<T>(baseCollection, null, null, null, action);
        }


        private class CustomActionCollection<T> : ICollection<T>
        {
            private readonly ICollection<T> _baseCollection;
            private readonly Action<T> _preAddAction;
            private readonly Action<T> _postAddAction;
            private readonly Action<T> _preRemoveAction;
            private readonly Action<T, bool> _postRemoveAction;


            public CustomActionCollection(
                [NotNull] ICollection<T> baseCollection,
                Action<T> preAddAction,
                Action<T> postAddAction,
                Action<T> preRemoveAction,
                Action<T, bool> postRemoveAction)
            {
                this._baseCollection = baseCollection;

                this._preAddAction = preAddAction ?? (_ => { });
                this._postAddAction = postAddAction ?? (_ => { });
                this._preRemoveAction = preRemoveAction ?? (_ => { });
                this._postRemoveAction = postRemoveAction ?? ((_, __) => { });
            }

            public int Count
            {
                get { return this._baseCollection.Count; }
            }

            public bool IsReadOnly
            {
                get { return this._baseCollection.IsReadOnly; }
            }


            public IEnumerator<T> GetEnumerator()
            {
                return this._baseCollection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public void Add(T item)
            {
                this._preAddAction(item);
                this._baseCollection.Add(item);
                this._postAddAction(item);
            }

            public void Clear()
            {
                this._baseCollection.Clear();
            }

            public bool Contains(T item)
            {
                return this._baseCollection.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                this._baseCollection.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item)
            {
                this._preRemoveAction(item);
                var result = this._baseCollection.Remove(item);
                this._postRemoveAction(item, result);
                return result;
            }
        }
    }
}
