using System.CodeDom;
using System.Reflection;

using CommonFramework.Maybe;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security._Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.DomainObjectToSecurity;

public abstract class DomainObjectToSecurityPropertyAssignerBase<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : SecurityServerPropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected abstract CodeExpression GetCondition(PropertyInfo property, bool isEdit);


    private CodeExpression GetCreateSecurityValueExpression(PropertyInfo property, CodeExpression resultVarDeclRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (resultVarDeclRef == null) throw new ArgumentNullException(nameof(resultVarDeclRef));

        return typeof(Maybe).ToTypeReferenceExpression()
                            .ToMethodReferenceExpression(nameof(Maybe.Return))
                            .ToMethodInvokeExpression(resultVarDeclRef);
    }

    protected sealed override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        var targetPropertyTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property);

        var resultVarDecl = targetPropertyTypeRef.ToVariableDeclarationStatement("result" + property.Name);
        var resultVarDeclRef = resultVarDecl.ToVariableReferenceExpression();



        return new CodeConditionStatement
               {
                       Condition = this.GetCondition(property, false),

                       TrueStatements =
                       {
                               resultVarDecl,
                               this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef, resultVarDeclRef),
                               this.GetCreateSecurityValueExpression(property, resultVarDeclRef).ToAssignStatement(targetPropertyRef)
                       },

                       FalseStatements =
                       {
                               targetPropertyTypeRef.ToNothingValueExpression().ToAssignStatement(targetPropertyRef)
                       }
               };
    }
}
