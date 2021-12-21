using System;
using System.Collections.Generic;

namespace Framework.Validation
{
    /// <summary>
    /// Метаданные валидируемого типа
    /// </summary>
    public interface IClassValidationMap// : IValidatorCollection<IClassValidator>
    {
        /// <summary>
        /// Имя валидируемого тип
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Валидируемый тип
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Список валидируемых свойств
        /// </summary>
        IReadOnlyCollection<IPropertyValidationMap> PropertyMaps { get;  }
    }

    /// <summary>
    /// Метаданные валидируемого типа
    /// </summary>
    /// <typeparam name="TSource">Валидируемый тип</typeparam>
    public interface IClassValidationMap<in TSource> : IClassValidationMap, IValidatorCollection<IClassValidator<TSource>>
    {
        /// <summary>
        /// Список валидируемых свойств
        /// </summary>
        new IReadOnlyCollection<IPropertyValidationMap<TSource>> PropertyMaps { get; }
    }
}