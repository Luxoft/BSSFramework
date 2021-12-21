using System;
using System.Collections.Generic;
using System.Reflection;

using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public partial interface ITargetSystemBLL
    {
        /// <summary>
        /// Регистрация базовых доменных типов (int, string, decimal, etc.)
        /// </summary>
        /// <param name="extBaseTypes">Дополнительные примитивные типы</param>
        /// <returns></returns>
        TargetSystem RegisterBase(IReadOnlyDictionary<Guid, Type> extBaseTypes = null);

        /// <summary>
        /// Регистрация доменных типов (Размеченных атрибутом DomainTypeAttribute) целевой систеы
        /// </summary>
        /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
        /// <param name="isMain">Целевая система являестся основной</param>
        /// <param name="isRevision">Система поддерживает ревизии</param>
        /// <param name="assemblies">Сборки, в которых ищутся доменные типы (по умолчанию сборка типа TPersistentDomainObjectBase)</param>
        /// <param name="extTypes">Дополнительные типы</param>
        /// <returns></returns>
        TargetSystem Register<TPersistentDomainObjectBase>(bool isMain, bool isRevision, IEnumerable<Assembly> assemblies = null, IReadOnlyDictionary<Guid, Type> extTypes = null);
    }
}