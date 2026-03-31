using System.CodeDom;
using System.Reflection;

using Framework.BLL.Domain.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.PropertyAssigner.__Base;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner._Security.Base;

public abstract class MaybePropertyAssigner<TConfiguration> : PropertyAssigner<TConfiguration>
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
{
    protected MaybePropertyAssigner(IPropertyAssigner<TConfiguration> innerAssigner)
            : base(innerAssigner)
    {
        if (innerAssigner == null) throw new ArgumentNullException(nameof(innerAssigner));

        this.InnerAssigner = innerAssigner;
    }


    public IPropertyAssigner InnerAssigner { get; }


    protected virtual bool IsMaybeProperty(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return this.Configuration.Environment.ExtendedMetadata.GetProperty(property).IsSecurity();
    }


    protected abstract CodeStatement GetSecurityAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef);

    public sealed override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (sourcePropertyRef == null) throw new ArgumentNullException(nameof(sourcePropertyRef));
        if (targetPropertyRef == null) throw new ArgumentNullException(nameof(targetPropertyRef));

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
