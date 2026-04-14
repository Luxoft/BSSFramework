using System.CodeDom;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;

namespace Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

public class ImplementFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public override FileType FileType { get; } = FileType.Implement;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Configuration.ImplementClassName,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsClass = true
        };

    protected override IEnumerable<CodeTypeMember> GetMembers() =>
        from methodGenerator in this.GetMethodGenerators()

        from method in methodGenerator.GetFacadeMethods()

        select method;
}
