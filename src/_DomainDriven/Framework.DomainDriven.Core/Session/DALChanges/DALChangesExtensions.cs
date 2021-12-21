using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public static class DALChangesExtensions
    {
        public static DALChanges<TResult> Select<TSource, TResult>([NotNull] this DALChanges<TSource> source, [NotNull] Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return new DALChanges<TResult>(source.CreatedItems.Select(selector), source.UpdatedItems.Select(selector), source.RemovedItems.Select(selector));
        }

        public static DALChanges<T> Where<T>([NotNull] this DALChanges<T> source, [NotNull] Func<T, bool> filter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return new DALChanges<T>(source.CreatedItems.Where(filter), source.UpdatedItems.Where(filter), source.RemovedItems.Where(filter));
        }

        public static Dictionary<T, DALObjectChangeType> ToChangeTypeDict<T>([NotNull] this DALChanges<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var request = source.CreatedItems.Select(item => new { Key = item, Value = DALObjectChangeType.Created })
                .Concat(source.UpdatedItems.Select(item => new { Key = item, Value = DALObjectChangeType.Updated }))
                .Concat(source.RemovedItems.Select(item => new { Key = item, Value = DALObjectChangeType.Removed }));

            return request.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static IEnumerable<TupleStruct<T, DALObjectChangeType>> ToPlainValues<T>([NotNull] this DALChanges<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var combined = source.CreatedItems.Select(item => TupleStruct.Create(item, DALObjectChangeType.Created))
                .Concat(source.UpdatedItems.Select(item => TupleStruct.Create(item, DALObjectChangeType.Updated)))
                .Concat(source.RemovedItems.Select(item => TupleStruct.Create(item, DALObjectChangeType.Removed)));

            return combined;
        }

        /// <summary>
        /// Схлопывание коллекции изменений по объекту
        /// </summary>
        public static DALChanges Composite(this IEnumerable<DALChanges> source)
        {
            var request = from dalChanges in source

                          from pair in dalChanges.ToChangeTypeDict()

                          group pair.Key.ToKeyValuePair(pair.Value) by pair.Key.Object into changeGroup

                          let finalState = changeGroup.ToDictionary().ToFinalState()

                          where finalState != null

                          select finalState.Value;

            return new DALChanges(request.ToDictionary());
        }

        /// <summary>
        /// Приведение череды изменения объекта к конечному состоянию
        /// </summary>
        /// <param name="states">Череда изменений объекта</param>
        private static KeyValuePair<IDALObject, DALObjectChangeType>? ToFinalState(this IReadOnlyDictionary<IDALObject, DALObjectChangeType> states)
        {
            if (states == null) throw new ArgumentNullException(nameof(states));

            var sourceCache = states.ToArray();

            var createState = states.SingleOrDefault(state => state.Value == DALObjectChangeType.Created);

            var removeState = states.SingleOrDefault(state => state.Value == DALObjectChangeType.Removed);

            if (!sourceCache.Any())
            {
                return null;
            }
            else if (!removeState.IsDefault())
            {
                if (!createState.IsDefault())
                {
                    return null;
                }
                else
                {
                    return removeState;
                }
            }
            else if (!createState.IsDefault())
            {
                return createState;
            }
            else
            {
                var updateStates = states.Where(state => state.Value == DALObjectChangeType.Updated).OrderByDescending(state => state.Key.ApplyIndex);

                return updateStates.First();
            }
        }

        public static ModificationType ToModificationType(this DALObjectChangeType changeType)
        {
            switch (changeType)
            {
                case DALObjectChangeType.Created:
                case DALObjectChangeType.Updated:
                    return ModificationType.Save;

                case DALObjectChangeType.Removed:
                    return ModificationType.Remove;

                default:
                    throw new ArgumentOutOfRangeException(nameof(changeType));
            }
        }
    }
}
