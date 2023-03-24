using System.CodeDom;

using Framework.DomainDriven.DTOGenerator.TypeScript.Configuration;
using Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces.Base;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.FileFactory.Interfaces;

/// <summary>
/// Default base audit persistent interfaceDTO file factory
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class DefaultBaseAuditPersistentInterfaceDTOFileFactory<TConfiguration> : InterfaceDTOFileFactory<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
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
