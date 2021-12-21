using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Workflow.Domain.Runtime
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Возвращает элемент с Definition
        /// </summary>
        /// <typeparam name="TSource">Тип коллекции элементов </typeparam>
        /// <typeparam name="TDefinition">Тип definition</typeparam>
        /// <param name="source">Коллекция элементов</param>
        /// <param name="definition">Definition</param>
        /// <returns>Словарь параметров</returns>
        public static TSource GetByDefinition<TSource, TDefinition>(this IEnumerable<TSource> source, TDefinition definition)
            where TSource : class, IDefinitionDomainObject<TDefinition>
            where TDefinition : AuditPersistentDomainObjectBase
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (definition == null) throw new ArgumentNullException(nameof(definition));

            return source.SingleOrDefault(element => element.Definition == definition);
        }
    }
}