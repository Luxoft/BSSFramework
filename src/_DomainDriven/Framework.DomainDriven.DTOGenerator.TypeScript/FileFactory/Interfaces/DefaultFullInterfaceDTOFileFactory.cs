using System;
using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces;

/// <summary>
/// Default full interfaceDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultFullInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
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
