using System.CodeDom;
using System.Reflection;

using Framework.BLL.Domain.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner.Security;

public abstract class MaybePropertyAssigner<TConfiguration>(IPropertyAssigner<TConfiguration> innerAssigner) : PropertyAssigner<TConfiguration>(innerAssigner)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    public IPropertyAssigner InnerAssigner { get; } = innerAssigner ?? throw new ArgumentNullException(nameof(innerAssigner));

    protected virtual bool IsMaybeProperty(PropertyInfo property)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));

        return this.Configuration.Environment.MetadataProxyProvider.Wrap(property).IsSecurity();
    }


    protected abstract CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef);

    public sealed override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property is null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef is null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef is null) throw new ArgumentNullException(nameof(targetPropertyRef));

        if (this.IsMaybeProperty(property))
        {
            return this.GetSecurityAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }
        else
        {
            return this.InnerAssigner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }
    }
}

