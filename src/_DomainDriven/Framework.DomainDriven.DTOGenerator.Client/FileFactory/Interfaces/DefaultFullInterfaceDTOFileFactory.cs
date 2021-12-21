using System;
using System.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultFullInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultFullInterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOInterfaceFileType FileType => ClientFileType.FullInterfaceDTO;

        public override CodeTypeReference BaseReference => this.Configuration.GetCodeTypeReference(this.DomainType, ClientFileType.SimpleInterfaceDTO);


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