using System;
using System.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultSimpleInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultSimpleInterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOInterfaceFileType FileType { get; } = ClientFileType.SimpleInterfaceDTO;

        public override CodeTypeReference BaseReference =>

            this.IsPersistent() ? this.Configuration.GetBaseAuditPersistentInterfaceReference() : this.Configuration.GetBaseAbstractInterfaceReference();


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