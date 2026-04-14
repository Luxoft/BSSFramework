using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class ValidatorFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    public override FileType FileType => FileType.Validator;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
        };

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ValidatorBase);
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ValidatorInterface);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        {
            var contextParameter = this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression("context");
            var cacheParameter = this.Configuration.GetCodeTypeReference(null, FileType.ValidatorCompileCache).ToParameterDeclarationExpression("cache");

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { contextParameter, cacheParameter },
                                 BaseConstructorArgs = { contextParameter.ToVariableReferenceExpression(), cacheParameter.ToVariableReferenceExpression() }
                         };
        }
    }
}
