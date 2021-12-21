using System.CodeDom;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces
{
    /// <summary>
    /// Default base abstract interfaceDTO file factory
    /// </summary>
    /// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
    public class DefaultBaseAbstractInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
    {
        public DefaultBaseAbstractInterfaceDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.DomainObjectBaseType)
        {
        }

        public override MainDTOInterfaceFileType FileType => ClientFileType.BaseAbstractInterfaceDTO;

        public override CodeTypeReference BaseReference => null;

        protected override CodeTypeDeclaration GetCodeTypeDeclaration()
        {
            return new CodeTypeDeclaration(this.Name)
            {
                IsInterface = true,
                IsPartial = true,
                Attributes = MemberAttributes.Public | MemberAttributes.Abstract
            };
        }

        protected override IEnumerable<CodeTypeReference> GetBaseTypes()
        {
            foreach (var baseType in base.GetBaseTypes())
            {
                yield return baseType;
            }
        }
    }
}
