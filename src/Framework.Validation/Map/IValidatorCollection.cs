using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Validation;

/// <summary>
/// Коллекция валидаторов
/// </summary>
/// <typeparam name="T">Тип валидатора</typeparam>
public interface IValidatorCollection<out T>
{
    /// <summary>
    /// Валидаторы
    /// </summary>
    IReadOnlyCollection<T> Validators { get; }
}
