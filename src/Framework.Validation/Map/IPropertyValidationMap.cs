using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework.Validation
{
    /// <summary>
    /// Метаданные валидируемого свойства
    /// </summary>
    public interface IPropertyValidationMap// : IValidatorCollection<IPropertyValidator>
    {
        /// <summary>
        /// Имя свойства
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Свойство
        /// </summary>
        PropertyInfo Property { get; }

        /// <summary>
        /// Свойство является коллекцией
        /// </summary>
        bool IsCollection { get; }

        /// <summary>
        /// Свойство является деталью
        /// </summary>
        bool IsDetail { get; }

        /// <summary>
        /// Свойство является композитным
        /// </summary>
        bool IsExpanded { get; }

        /// <summary>
        /// Метаданные типа
        /// </summary>
        IClassValidationMap ReflectedTypeMap { get; }

        /// <summary>
        /// Метаданные типа свойства
        /// </summary>
        IClassValidationMap PropertyTypeMap { get; }
    }

    /// <summary>
    /// Метаданные валидируемого свойства
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    public interface IPropertyValidationMap<in TSource> : IPropertyValidationMap
    {
        /// <summary>
        /// Метаданные типа, которому принадлежит свойство
        /// </summary>
        new IClassValidationMap<TSource> ReflectedTypeMap { get; }
    }

    /// <summary>
    /// Метаданные валидируемого свойства
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    public interface IPropertyValidationMap<in TSource, in TProperty> : IPropertyValidationMap<TSource>, IValidatorCollection<IPropertyValidator<TSource, TProperty>>
    {
    }

    /// <summary>
    /// Метаданные валидируемого одиночного-свойства
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    public interface ISinglePropertyValidationMap<in TSource, in TProperty> : IPropertyValidationMap<TSource, TProperty>
    {
        /// <summary>
        /// Метаданные типа свойства
        /// </summary>
        new IClassValidationMap<TProperty> PropertyTypeMap { get; }
    }

    /// <summary>
    /// Метаданные валидируемого свойства-коллекции
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <typeparam name="TElement">Тип элемента коллекции</typeparam>
    public interface ICollectionPropertyValidationMap<in TSource, in TProperty, in TElement> : IPropertyValidationMap<TSource, TProperty>
        where TProperty : IEnumerable<TElement>
    {
        /// <summary>
        /// Метаданные типа свойства
        /// </summary>
        new IClassValidationMap<TElement> PropertyTypeMap { get; }
    }
}