using System.CodeDom;
using System.Reflection;

using CommonFramework.Maybe;

using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;
using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.SecurityToDomainObject.Strict;

public abstract class StrictToDomainObjectPropertyAssignerBase<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected abstract CodeExpression GetCondition(PropertyInfo property);

    protected override CodeStatement GetSecurityAssignStatementInternal(PropertyInfo property, CodeExpression justValueRefExpr, CodeStatement innerAssignStatement)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (justValueRefExpr == null) throw new ArgumentNullException(nameof(justValueRefExpr));
        if (innerAssignStatement == null) throw new ArgumentNullException(nameof(innerAssignStatement));

        return new CodeConditionStatement(justValueRefExpr.ToPropertyReference(nameof(Maybe<>.HasValue)))
               {
                   TrueStatements =
                   {
                       new CodeConditionStatement
                       {
                           Condition = this.GetCondition(property),

                           TrueStatements = { innerAssignStatement },

                           FalseStatements = { new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(BusinessLogicException),
                                                                                                              new CodePrimitiveExpression($"Access for write to field \"{property.Name}\" denied"))) }
                       }
                   }
               };
    }
}
