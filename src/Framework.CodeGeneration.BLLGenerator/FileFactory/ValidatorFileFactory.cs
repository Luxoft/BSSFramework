using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class ValidatorFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType.FileType FileType => BLLGenerator.FileType.FileType.Validator;

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
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, BLLGenerator.FileType.FileType.ValidatorBase);
        yield return this.Configuration.GetCodeTypeReference(this.DomainType, BLLGenerator.FileType.FileType.ValidatorInterface);
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }

        {
            var contextParameter = this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression("context");
            var cacheParameter = this.Configuration.GetCodeTypeReference(null, BLLGenerator.FileType.FileType.ValidatorCompileCache).ToParameterDeclarationExpression("cache");

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { contextParameter, cacheParameter },
                                 BaseConstructorArgs = { contextParameter.ToVariableReferenceExpression(), cacheParameter.ToVariableReferenceExpression() }
                         };
        }
    }
}
