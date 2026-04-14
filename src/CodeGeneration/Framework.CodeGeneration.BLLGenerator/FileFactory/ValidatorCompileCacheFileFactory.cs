using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.Validation;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class ValidatorCompileCacheFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    public override FileType FileType => FileType.ValidatorCompileCache;

    public override CodeTypeReference BaseReference { get; } = typeof(ValidatorCompileCache).ToTypeReference();

    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
        };

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        var validationMapParam =
                this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ValidationMap)
                    .ToParameterDeclarationExpression("validationMap");

        yield return new CodeConstructor
                     {
                         Attributes = MemberAttributes.Public,
                         Parameters = { validationMapParam },
                         BaseConstructorArgs = { validationMapParam.ToVariableReferenceExpression() }
                     };
    }
}
