using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript
{
    /// <summary>
    /// IClient base generator configuration
    /// </summary>
    /// <typeparam name="TEnvironmentBase">The type of the environment base.</typeparam>
    public interface ITypeScriptDTOGeneratorConfiguration<out TEnvironmentBase> : ITypeScriptDTOGeneratorConfiguration, IGeneratorConfigurationBase<TEnvironmentBase>
        where TEnvironmentBase : ITypeScriptGenerationEnvironmentBase
    {
    }

    /// <summary>
    /// IClient base generator configuration
    /// </summary>
    public interface ITypeScriptDTOGeneratorConfiguration : IGeneratorConfigurationBase
    {
        /// <summary>
        /// Флаг генерации клиентского маппинг-сервиса (необходим для работы UpdateDTO). Можно включать после завершения задачи: #IADFRAME-1624
        /// </summary>
        bool GenerateClientMappingService { get; }

        bool ContainsPropertyChange { get; }

        ReadOnlyCollection<Assembly> ReuseTypesAssemblies { get; }

        ReadOnlyCollection<Type> ReferencedTypes { get; }

        ReadOnlyCollection<Type> EnumTypes { get; }

        ReadOnlyCollection<Type> ClassTypes { get; }

        ReadOnlyCollection<Type> StructTypes { get; }

        MainDTOInterfaceFileType GetBaseInterfaceType(MainDTOFileType fileType);

        MainDTOFileType GetImplementType([NotNull] MainDTOInterfaceFileType fileType);

        IEnumerable<CodeTypeMember> GetFileFactoryExtendedMembers([NotNull] ICodeFileFactory<FileType> fileFactory);

        IEnumerable<string> GetNamespaces();

        IEnumerable<RequireJsModule> GetModules();
    }
}
