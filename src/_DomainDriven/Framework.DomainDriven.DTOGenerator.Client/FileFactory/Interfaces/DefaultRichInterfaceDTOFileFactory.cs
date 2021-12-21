using System;
using System.CodeDom;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class DefaultRichInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        public DefaultRichInterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }


        public override MainDTOInterfaceFileType FileType { get; } = ClientFileType.RichInterfaceDTO;


        public override CodeTypeReference BaseReference => this.Configuration.GetCodeTypeReference(this.DomainType, ClientFileType.FullInterfaceDTO);


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