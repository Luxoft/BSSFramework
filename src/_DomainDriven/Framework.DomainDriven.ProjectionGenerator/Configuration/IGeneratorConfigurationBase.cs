using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ProjectionGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
{
}

public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    /// <summary>
    /// Генерировать set-акцессор для IsOneToOne-свойств
    /// </summary>
    bool OneToOneSetter { get; }

    /// <summary>
    /// Генерирование публичных конструкторов у проекций
    /// </summary>
    bool GeneratePublicCtors { get; }

    /// <summary>
    /// Получение генрируемых аттрибутов для проекционного типа
    /// </summary>
    /// <param name="domainType">Свойство</param>
    /// <returns></returns>
    IEnumerable<CodeAttributeDeclaration> GetDomainTypeAttributeDeclarations(Type domainType);

    /// <summary>
    /// Получение генрируемых аттрибутов для проекционного свойства
    /// </summary>
    /// <param name="property">Свойство</param>
    /// <returns></returns>
    IEnumerable<CodeAttributeDeclaration> GetPropertyAttributeDeclarations(PropertyInfo property);
}
