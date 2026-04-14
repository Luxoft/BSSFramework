using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class ValidationMapFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    public override FileType FileType => FileType.ValidationMap;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
        };

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, FileType.ValidationMapBase);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        {
            var extendedValidationDataParam =
                    typeof(IServiceProvider).ToTypeReference().ToParameterDeclarationExpression("serviceProvider");


            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { extendedValidationDataParam },
                                 BaseConstructorArgs = { extendedValidationDataParam.ToVariableReferenceExpression() }
                         };
        }
    }
}
