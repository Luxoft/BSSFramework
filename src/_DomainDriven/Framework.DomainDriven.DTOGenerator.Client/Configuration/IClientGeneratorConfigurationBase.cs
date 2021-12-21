using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public interface IClientGeneratorConfigurationBase<out TEnvironmentBase> : IClientGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironmentBase>
        where TEnvironmentBase : IClientGenerationEnvironmentBase
    {

    }

    public interface IClientGeneratorConfigurationBase : IGeneratorConfigurationBase
    {
        bool ContainsPropertyChange { get; }


        ReadOnlyCollection<Assembly> ReuseTypesAssemblies { get; }


        ReadOnlyCollection<Type> ReferencedTypes { get; }


        ReadOnlyCollection<Type> EnumTypes { get; }

        ReadOnlyCollection<Type> ClassTypes { get; }

        ReadOnlyCollection<Type> StructTypes { get; }

        FileType GetBaseInterfaceType(MainDTOFileType fileType, bool raiseIfNull = false);

        IEnumerable<CodeTypeMember> GetFileFactoryExtendedMembers([NotNull] ICodeFileFactory<FileType> fileFactory);
    }
}
