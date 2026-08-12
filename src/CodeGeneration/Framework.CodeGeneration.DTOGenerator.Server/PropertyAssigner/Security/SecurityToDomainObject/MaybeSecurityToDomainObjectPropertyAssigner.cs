using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.Core;
using Framework.Database.Attributes;

namespace Framework.CodeGeneration.DTOGenerator.Server.PropertyAssigner.Security.SecurityToDomainObject;

public abstract class MaybeSecurityToDomainObjectPropertyAssigner<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner)
    : SecurityServerPropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
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
