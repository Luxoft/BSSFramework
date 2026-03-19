using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Server;

public abstract class MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration> : SecurityServerPropertyAssigner<TConfiguration>
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    protected MaybeSecurityToDomainObjectPropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner)
        : base(innerAssigner)
    {
    }

    protected abstract CodeStatement GetSecurityAssignStatementInternal(PropertyInfo property, CodeExpression justValueRefExpr, CodeStatement innerAssignStatement);

    protected sealed override CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property.HasAttribute<VersionAttribute>())
        {
            return this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }
        else
        {

            return this.GetSecurityAssignStatementInternal(
                property,
                sourcePropertyRef,
                this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef.ToPropertyReference("Value"), targetPropertyRef));

            //return this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef.ToPropertyReference(nameof(Maybe<>.Value)), targetPropertyRef);
        }
    }
}
