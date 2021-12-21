using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Helpers;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces
{
    /// <summary>
    /// Default rich interfaceDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultRichInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultRichInterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override MainDTOInterfaceFileType FileType => ClientFileType.RichInterfaceDTO;

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

        protected override IEnumerable<CodeTypeMember> GetMembers()
        {
            return from propertyType in this.Configuration.GetDomainTypeProperties(this.DomainType, this.Configuration.GetImplementType(this.FileType))
                   select new CodeMemberProperty
                   {
                       Name = propertyType.Name,
                       Type = this.SimplifyCodeTypeReference(propertyType),
                       HasGet = true,
                       HasSet = true,
                   };
        }
    }
}
