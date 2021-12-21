using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public abstract class InterfaceDTOFileFactory<TConfiguration> : DTOFileFactory<TConfiguration, MainDTOInterfaceFileType>
        where TConfiguration : class, IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase>
    {
        protected InterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
            this.CodeTypeReferenceService = new MainCodeTypeReferenceService<TConfiguration>(this.Configuration);
        }


        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            return from propertyType in this.Configuration.GetDomainTypeProperties(this.DomainType, this.FileType.MainType)

                   select new CodeMemberProperty
                   {
                       Name = propertyType.Name,
                       Type = this.CodeTypeReferenceService.GetCodeTypeReference(propertyType, true),
                       HasGet = true,
                       HasSet = true,
                   };
        }

        protected override IEnumerable<CodeAttributeDeclaration> GetCustomAttributes()
        {
            yield break;
        }
    }
}