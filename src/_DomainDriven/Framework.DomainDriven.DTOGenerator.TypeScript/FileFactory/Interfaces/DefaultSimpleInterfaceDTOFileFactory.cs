using System;
using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces
{
    /// <summary>
    /// Default simple interfaceDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultSimpleInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultSimpleInterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override MainDTOInterfaceFileType FileType => ClientFileType.SimpleInterfaceDTO;

        public override CodeTypeReference BaseReference
            => this.Configuration.GetCodeTypeReference(this.DomainType,
                                                       this.IsPersistent() ? ClientFileType.BaseAuditPersistentInterfaceDTO : ClientFileType.BaseAbstractInterfaceDTO);

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsInterface = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public | MemberAttributes.Abstract
            };
        }
    }
}
