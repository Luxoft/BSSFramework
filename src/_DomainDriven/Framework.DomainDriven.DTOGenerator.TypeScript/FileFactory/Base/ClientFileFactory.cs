using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base
{
    /// <summary>
    /// IClient file factory
    /// </summary>
    public interface IClientFileFactory
    {
        CodeTypeReference CurrentInterfaceReference { get; }

        CodeTypeReference BaseAbstractInterfaceReference { get; }

        CodeTypeReference BasePersistentInterfaceReference { get; }
    }

    /// <summary>
    /// IClient file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    /// <typeparam name="TFileType">The type of the file type.</typeparam>
    public interface IClientFileFactory<out TConfiguration, out TFileType> : IFileFactory<TConfiguration, TFileType>, IClientFileFactory
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
        where TFileType : DTOFileType
    {

    }

    /// <summary>
    /// Client file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    /// <typeparam name="TFileType">The type of the file type.</typeparam>
    public abstract class ClientFileFactory<TConfiguration, TFileType> : FileFactory<TConfiguration, TFileType>, IClientFileFactory<TConfiguration, TFileType>, IDTOSource<TConfiguration>
        where TFileType : DTOFileType
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        protected ClientFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public CodeTypeReference CurrentInterfaceReference
        {
            get { return (this.FileType as MainDTOFileType).Maybe(fileType => this.Configuration.GetCodeTypeReference(this.DomainType, this.Configuration.GetBaseInterfaceType(fileType))); }
        }

        public CodeTypeReference BaseAbstractInterfaceReference => this.Configuration.GetCodeTypeReference(this.Configuration.Environment.DomainObjectBaseType, ClientFileType.BaseAbstractInterfaceDTO);

        public CodeTypeReference BasePersistentInterfaceReference => this.Configuration.GetCodeTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType, ClientFileType.BasePersistentInterfaceDTO);

        public CodeTypeReference BaseAuditPersistentInterfaceReference => this.Configuration.GetCodeTypeReference(this.Configuration.Environment.AuditPersistentDomainObjectBaseType, ClientFileType.BaseAuditPersistentInterfaceDTO);

        DTOFileType IDTOSource.FileType => this.FileType;

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            foreach (var baseMember in base.GetMembers())
            {
                yield return baseMember;
            }

            foreach (var member in this.Configuration.GetFileFactoryExtendedMembers(this))
            {
                yield return member;
            }
        }
    }
}
