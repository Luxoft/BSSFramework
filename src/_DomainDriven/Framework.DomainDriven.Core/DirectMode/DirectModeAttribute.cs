using System;

namespace Framework.DomainDriven
{
    /// <summary>
    /// Атрибут управления генерацией по модели
    /// </summary>
    public class DirectModeAttribute : Attribute
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="directMode">Параметры управления генерацией по модели</param>
        public DirectModeAttribute(DirectMode directMode)
        {
            this.DirectMode = directMode;
        }

        /// <summary>
        /// Параметры управления генерацией по модели
        /// </summary>
        public DirectMode DirectMode { get; }
    }
}
