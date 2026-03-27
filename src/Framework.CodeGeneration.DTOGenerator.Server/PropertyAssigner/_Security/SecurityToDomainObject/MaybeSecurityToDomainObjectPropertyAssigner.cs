using System.CodeDom;
using System.Reflection;

using Framework.Application.Domain.Attributes;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security._Base;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner._Security.SecurityToDomainObject;

public abstract class MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : SecurityServerPropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
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
