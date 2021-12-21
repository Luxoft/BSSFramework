using System;

namespace Framework.Validation
{
    /// <summary>
    /// Атрибут для выборочной валидации свойства
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyValidationModeAttribute : Attribute
    {
        /// <summary>
        /// Режим валидации свойства (по умолчанию выключена для простых виртуальных свойств)
        /// </summary>
        public readonly PropertyValidationMode Mode;

        /// <summary>
        /// Режим валидации внутренних объектов (по умолчанию включена только для Detail-свойств)
        /// </summary>
        public readonly PropertyValidationMode DeepMode;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="mode">Режим валидации свойства</param>
        /// <param name="deepMode">Режим валидации внутренних объектов</param>
        public PropertyValidationModeAttribute(PropertyValidationMode mode, PropertyValidationMode deepMode = PropertyValidationMode.Auto)
        {
            this.Mode = mode;
            this.DeepMode = deepMode;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="enabled">Режим валидации свойства</param>
        public PropertyValidationModeAttribute(bool enabled)
            : this(enabled.ToPropertyValidationMode())
        {
        }

        /// <summary>
        /// Проверка на указание явной валидации
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        public bool HasValue(bool value)
        {
            return this.Mode == value.ToPropertyValidationMode();
        }

        /// <summary>
        /// Проверка на указание явной валидации внутренного объекта
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns></returns>
        public bool HasDeepValue(bool value)
        {
            return this.DeepMode == value.ToPropertyValidationMode();
        }
    }
}