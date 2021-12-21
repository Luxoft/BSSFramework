using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Lambda
{
    /// <inheritdoc />
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    public class ProjectionCustomProperty<TProperty> : IProjectionCustomProperty
    {
        private readonly Lazy<TypeReferenceBase> lazyType;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя свойства</param>
        /// <param name="writable">Доступна запись в свойство</param>
        /// <param name="getPropProjection">Проекция на тип свойства</param>
        /// <param name="fetchs">Дополнительная подгрузка свойств</param>
        /// <param name="attributes">Дополнительные атрибуты генерации</param>
        public ProjectionCustomProperty([NotNull] string name, bool writable = false, Func<Projection<TProperty>> getPropProjection = null, Type collectionType = null, IEnumerable<string> fetchs = null, IEnumerable<Attribute> attributes = null)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Value cannot be null or whitespace.", nameof(name)); }

            this.Name = name;
            this.Writable = writable;
            this.lazyType = LazyHelper.Create(() => getPropProjection == null ? (TypeReferenceBase)new TypeReferenceBase.FixedTypeReference(collectionType.SafeMakeProjectionCollectionType(typeof(TProperty)))
                                                                              : new TypeReferenceBase.BuildTypeReference(typeof(TProperty), collectionType, false, getPropProjection()));

            this.Fetchs = fetchs.EmptyIfNull().ToList();
            this.Attributes = attributes.EmptyIfNull().ToList();
        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public bool Writable { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> Fetchs { get; }

        /// <inheritdoc />
        public IReadOnlyList<Attribute> Attributes { get; }

        /// <inheritdoc />
        public TypeReferenceBase Type => this.lazyType.Value;
    }
}
