using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.Validation;

namespace Framework.DomainDriven.BLLGenerator;

public class ValidatorBaseFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType FileType => FileType.ValidatorBase;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       TypeAttributes = TypeAttributes.Public | TypeAttributes.Abstract,
                       IsPartial = true,
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return typeof(BLLContextHandlerValidator<,>).ToTypeReference(this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference, this.Configuration.OperationContextType.ToTypeReference());
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
    {
        foreach (var member in base.GetMembers())
        {
            yield return member;
        }


        {
            var contextParameter = this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference.ToParameterDeclarationExpression("context");
            var cacheParameter = typeof(ValidatorCompileCache).ToTypeReference().ToParameterDeclarationExpression("cache");

            var statements = from domainType in this.Configuration.ValidationTypes

                             select new CodeBaseReferenceExpression().ToMethodReferenceExpression("RegisterHandler", domainType.ToTypeReference())
                                                                     .ToMethodInvokeExpression(new CodeThisReferenceExpression().ToPropertyReference($"Get{domainType.Name}ValidationResult"))
                                                                     .ToExpressionStatement();

            yield return new CodeConstructor
                         {
                                 Attributes = MemberAttributes.Public,
                                 Parameters = { contextParameter, cacheParameter },
                                 BaseConstructorArgs = { contextParameter.ToVariableReferenceExpression(), cacheParameter.ToVariableReferenceExpression() }
                         }.WithStatements(statements);
        }

        foreach (var domainType in this.Configuration.ValidationTypes)
        {
            var sourceParameter = domainType.ToTypeReference().ToParameterDeclarationExpression("source");

            var operationContextParameter = this.Configuration.OperationContextType.ToTypeReference().ToParameterDeclarationExpression("operationContext");

            var ownerStateParameter = typeof(IValidationState).ToTypeReference().ToParameterDeclarationExpression("ownerState");

            yield return new CodeMemberMethod
                         {
                                 Attributes = MemberAttributes.Family,
                                 Name = $"Get{domainType.Name}ValidationResult",
                                 ReturnType = typeof(ValidationResult).ToTypeReference(),
                                 Parameters = { sourceParameter, operationContextParameter, ownerStateParameter },

                                 Statements = { new CodeBaseReferenceExpression().ToMethodInvokeExpression(
                                                 "GetValidationResult",
                                                 sourceParameter.ToVariableReferenceExpression(),
                                                 operationContextParameter.ToVariableReferenceExpression(),
                                                 ownerStateParameter.ToVariableReferenceExpression(),
                                                 false.ToPrimitiveExpression()).ToMethodReturnStatement() }
                         };
        }
    }
}
