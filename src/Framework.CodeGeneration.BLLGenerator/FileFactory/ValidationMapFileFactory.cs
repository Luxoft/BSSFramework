using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class ValidationMapFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType.FileType FileType => BLLGenerator.FileType.FileType.ValidationMap;

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
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, BLLGenerator.FileType.FileType.ValidationMapBase);
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
