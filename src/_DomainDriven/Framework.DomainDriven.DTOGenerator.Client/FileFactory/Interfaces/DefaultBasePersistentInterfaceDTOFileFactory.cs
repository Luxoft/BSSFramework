using System.CodeDom;
using System.ComponentModel;
using Framework.Reactive;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultBasePersistentInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultBasePersistentInterfaceDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
        {
        }


        public override MainDTOInterfaceFileType FileType { get; } = ClientFileType.BasePersistentInterfaceDTO;

        public override CodeTypeReference BaseReference => this.Configuration.GetBaseAbstractInterfaceReference();


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

    public class DefaultBaseAuditPersistentInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultBaseAuditPersistentInterfaceDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
        {
        }


        public override MainDTOInterfaceFileType FileType { get; } = ClientFileType.BaseAuditPersistentInterfaceDTO;

        public override CodeTypeReference BaseReference => this.Configuration.GetBasePersistentInterfaceReference();


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