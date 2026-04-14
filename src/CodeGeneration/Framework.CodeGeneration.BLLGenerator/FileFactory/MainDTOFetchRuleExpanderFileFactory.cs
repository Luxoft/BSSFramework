using System.CodeDom;

using Framework.CodeGeneration.BLLGenerator.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class MainDTOFetchRuleExpanderFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    public override FileType FileType => FileType.MainDTOFetchRuleExpander;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
        };

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, FileType.MainDTOFetchRuleExpanderBase);
    }
}
