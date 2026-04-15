using System.CodeDom;
using System.Reflection;

using CommonFramework;

using Framework.Application;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security.SecurityToDomainObject.Strict;

public abstract class StrictToDomainObjectPropertyAssignerBase<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
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
