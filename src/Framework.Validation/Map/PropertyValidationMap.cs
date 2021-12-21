using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;

namespace Framework.Validation
{
    /// <summary>
    /// Метаданные валидируемого свойства
    /// </summary>
    public abstract class PropertyValidationMap : IPropertyValidationMap, IValidatorCollection<IPropertyValidator>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="property">Свойство</param>
        protected PropertyValidationMap(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            this.Property = property;
            this.PropertyName = this.Property.GetValidationName();
        }

        /// <inheritdoc />
        public PropertyInfo Property { get; set; }

        /// <inheritdoc />
        public string PropertyName { get; }

        /// <inheritdoc />
        public abstract bool IsDetail { get; }

        /// <inheritdoc />
        public bool IsExpanded => this.BasePropertyTypeMap.Type.HasExpandValidation();

        /// <inheritdoc />
        public abstract bool IsCollection { get; }

        /// <summary>
        /// Метаданные типа свойства
        /// </summary>
        protected abstract IClassValidationMap BasePropertyTypeMap { get; }

        /// <summary>
        /// Метаданные типа
        /// </summary>
        protected abstract IClassValidationMap BaseReflectedTypeMap { get; }

        /// <inheritdoc />
        protected abstract IReadOnlyCollection<IPropertyValidator> BaseValidators { get; }


        IClassValidationMap IPropertyValidationMap.PropertyTypeMap => this.BasePropertyTypeMap;

        IClassValidationMap IPropertyValidationMap.ReflectedTypeMap => this.BaseReflectedTypeMap;

        IReadOnlyCollection<IPropertyValidator> IValidatorCollection<IPropertyValidator>.Validators => this.BaseValidators;
    }

    /// <summary>
    /// Метаданные валидируемого свойства
    /// </summary>
    /// <typeparam name="TSource">Тип объекта</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    public abstract class PropertyValidationMap<TSource, TProperty> : PropertyValidationMap, IPropertyValidationMap<TSource, TProperty>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="property">Свойство</param>
        /// <param name="reflectedTypeMap">Метаданные типа</param>
        /// <param name="validators">Валидаторы</param>
        protected PropertyValidationMap(PropertyInfo property, IClassValidationMap<TSource> reflectedTypeMap, IEnumerable<IPropertyValidator<TSource, TProperty>> validators)
            : base(property)
        {
            if (reflectedTypeMap == null) throw new ArgumentNullException(nameof(reflectedTypeMap));
            if (validators == null) throw new ArgumentNullException(nameof(validators));

            this.ReflectedTypeMap = reflectedTypeMap;

            this.Validators = validators.ToReadOnlyCollection();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IPropertyValidator<TSource, TProperty>> Validators { get; }

        /// <inheritdoc />
        public IClassValidationMap<TSource> ReflectedTypeMap { get; }

        /// <inheritdoc />
        protected override IClassValidationMap BaseReflectedTypeMap => this.ReflectedTypeMap;

        /// <inheritdoc />
        protected override IReadOnlyCollection<IPropertyValidator> BaseValidators => this.Validators;


        IClassValidationMap<TSource> IPropertyValidationMap<TSource>.ReflectedTypeMap => this.ReflectedTypeMap;
    }

    /// <summary>
    /// Метаданные валидируемого одиночного-свойства
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    public class SinglePropertyValidationMap<TSource, TProperty> : PropertyValidationMap<TSource, TProperty>, ISinglePropertyValidationMap<TSource, TProperty>
    {
        public SinglePropertyValidationMap(
                Expression<Func<TSource, TProperty>> propertyExpr,
                IClassValidationMap<TSource> reflectedTypeMap,
                IEnumerable<IPropertyValidator<TSource, TProperty>> validators,
                IClassValidationMap<TProperty> propertyTypeMap)
            : this(propertyExpr.UpdateBody(FixPropertySourceVisitor.Value).GetProperty(), reflectedTypeMap, validators, propertyTypeMap)
        {
        }

        public SinglePropertyValidationMap(
                PropertyInfo property,
                IClassValidationMap<TSource> reflectedTypeMap,
                IEnumerable<IPropertyValidator<TSource, TProperty>> validators,
                IClassValidationMap<TProperty> propertyTypeMap)
            : base(property, reflectedTypeMap, validators)
        {
            if (propertyTypeMap == null) throw new ArgumentNullException(nameof(propertyTypeMap));

            this.PropertyTypeMap = propertyTypeMap;

            this.IsDetail = property.IsDetail();
        }

        public override bool IsCollection { get; } = false;

        public override bool IsDetail { get; }

        public IClassValidationMap<TProperty> PropertyTypeMap { get; }

        protected override IClassValidationMap BasePropertyTypeMap => this.PropertyTypeMap;

        IClassValidationMap<TProperty> ISinglePropertyValidationMap<TSource, TProperty>.PropertyTypeMap => this.PropertyTypeMap;
    }

    /// <summary>
    /// Метаданные валидируемого свойства-коллекции
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <typeparam name="TElement">Тип элемента коллекции</typeparam>
    public class CollectionPropertyValidationMap<TSource, TProperty, TElement> : PropertyValidationMap<TSource, TProperty>,
                                                                                 ICollectionPropertyValidationMap<TSource, TProperty, TElement>

        where TProperty : IEnumerable<TElement>
    {
        public CollectionPropertyValidationMap(Expression<Func<TSource, TProperty>> propertyExpr, IClassValidationMap<TSource> reflectedTypeMap, IEnumerable<IPropertyValidator<TSource, TProperty>> validators, IClassValidationMap<TElement> propertyElementTypeMap)
            : this(propertyExpr.UpdateBody(FixPropertySourceVisitor.Value).GetProperty(), reflectedTypeMap, validators, propertyElementTypeMap)
        {
        }

        public CollectionPropertyValidationMap(PropertyInfo property, IClassValidationMap<TSource> reflectedTypeMap, IEnumerable<IPropertyValidator<TSource, TProperty>> validators, IClassValidationMap<TElement> propertyElementTypeMap)
            : base(property, reflectedTypeMap, validators)
        {
            if (propertyElementTypeMap == null) throw new ArgumentNullException(nameof(propertyElementTypeMap));

            this.PropertyElementTypeMap = propertyElementTypeMap;
            this.IsDetail = !property.IsNotDetail();
        }


        public override bool IsCollection { get; } = true;

        public override bool IsDetail { get; }

        public IClassValidationMap<TElement> PropertyElementTypeMap { get; }


        protected override IClassValidationMap BasePropertyTypeMap => this.PropertyElementTypeMap;


        IClassValidationMap<TElement> ICollectionPropertyValidationMap<TSource, TProperty, TElement>.PropertyTypeMap => this.PropertyElementTypeMap;
    }
}