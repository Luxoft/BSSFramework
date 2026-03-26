using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Exceptions;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class ExpandMaybeSecurityToDomainObjectPropertyAssignerBase<TConfiguration> : SecurityServerPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected ExpandMaybeSecurityToDomainObjectPropertyAssignerBase(IPropertyAssigner<TConfiguration> innerAssigner)
            : base(innerAssigner)
    {
    }


    protected abstract CodeExpression GetCondition(PropertyInfo property);

    protected override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

        var sourcePropertyTypeRef = this.CodeTypeReferenceService.GetCodeTypeReference(property);

        return new CodeConditionStatement
               {
                       Condition = new CodeValueEqualityOperatorExpression(sourcePropertyRef, new CodeDefaultValueExpression(sourcePropertyTypeRef)).ToNegateExpression(),
                       TrueStatements =
                       {
                               new CodeConditionStatement
                               {
                                       Condition = this.GetCondition(property),

                                       TrueStatements =
                                       {
                                               this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef)
                                       },

                                       FalseStatements = { new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(BusinessLogicException),
                                                               new CodePrimitiveExpression($"Access for write to field \"{property.Name}\" denied"))) }
                               }
                       }
               };
    }
}
