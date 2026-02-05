using System.CodeDom;

namespace Framework.DomainDriven.BLLGenerator;

public class MainDTOFetchRuleExpanderFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType FileType => FileType.MainDTOFetchRuleExpander;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, FileType.MainDTOFetchRuleExpanderBase);
    }
}
