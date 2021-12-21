using System;
using System.CodeDom;
using System.Linq;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base
{
    /// <summary>
    /// InterfaceDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public abstract class InterfaceDTOFileFactory<TConfiguration> : ClientFileFactory<TConfiguration, MainDTOInterfaceFileType>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        protected InterfaceDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
        {
        }

        public override IPropertyCodeTypeReferenceService CodeTypeReferenceService => new MainCodeTypeReferenceService<TConfiguration>(this.Configuration);

        protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
        {
            return from propertyType in this.Configuration.GetDomainTypeProperties(this.DomainType, this.Configuration.GetImplementType(this.FileType))
                   select new CodeMemberProperty
                   {
                       Name = propertyType.Name,
                       Type = this.CodeTypeReferenceService.GetCodeTypeReference(propertyType, Constants.UseSecurity),
                       HasGet = true,
                       HasSet = true,
                   };
        }
    }
}
