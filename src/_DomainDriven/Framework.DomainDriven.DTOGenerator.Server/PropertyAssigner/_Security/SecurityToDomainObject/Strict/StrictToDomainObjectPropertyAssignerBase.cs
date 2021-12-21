using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Exceptions;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public abstract class StrictToDomainObjectPropertyAssignerBase<TConfiguration> : MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
    {
        protected StrictToDomainObjectPropertyAssignerBase(IPropertyAssigner<TConfiguration> innerAssigner)
            : base(innerAssigner)
        {
        }


        protected abstract CodeExpression GetCondition(PropertyInfo property);

        protected override CodeStatement GetSecurityAssignStatementInternal(PropertyInfo property, CodeExpression justValueRefExpr, CodeStatement innerAssignStatement)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (justValueRefExpr == null) throw new ArgumentNullException(nameof(justValueRefExpr));
            if (innerAssignStatement == null) throw new ArgumentNullException(nameof(innerAssignStatement));

            return new CodeNotNullConditionStatement(justValueRefExpr)
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
}