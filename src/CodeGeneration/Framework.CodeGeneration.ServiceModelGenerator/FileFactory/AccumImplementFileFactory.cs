using System.CodeDom;

using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.FileFactory;

public class AccumImplementFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, typeof(object))
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
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
        from methodGenerator in this.Configuration.GetAccumMethodGenerators()

        from method in methodGenerator.GetFacadeMethods()

        select method;
}
