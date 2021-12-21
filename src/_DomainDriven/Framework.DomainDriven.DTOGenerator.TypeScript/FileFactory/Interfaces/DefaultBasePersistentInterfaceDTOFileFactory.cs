using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces
{
    /// <summary>
    /// Default base persistent interfaceDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBasePersistentInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBasePersistentInterfaceDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.PersistentDomainObjectBaseType)
        {
        }

        public override MainDTOInterfaceFileType FileType => ClientFileType.BasePersistentInterfaceDTO;

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
}