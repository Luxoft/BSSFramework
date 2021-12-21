using System;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.Projection.Lambda
{
    /// <summary>
    /// Интерфейс проекционного свойства
    /// </summary>
    public interface IProjectionProperty : IProjectionAttributeProvider
    {
        /// <summary>
        /// Имя свойства
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Роль свойства
        /// </summary>
        ProjectionPropertyRole Role { get; }

        /// <summary>
        /// Expression-путь до свойства
        /// </summary>
        LambdaExpression Expression { get; }

        /// <summary>
        /// Property-путь до свойства
        /// </summary>
        PropertyPath Path { get; }

        /// <summary>
        /// Исходный тип от которого строится проекция
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// Тип свойства проекции
        /// </summary>
        TypeReferenceBase.BuildTypeReference Type { get; }

        /// <summary>
        /// Отключение сериализации свойства в DTO
        /// </summary>
        bool IgnoreSerialization { get; }

        /// <summary>
        /// Виртуальное свойство интерфейса, которое имплементируется explicit-ом
        /// </summary>
        PropertyInfo VirtualExplicitInterfaceProperty { get; }
    }
}
