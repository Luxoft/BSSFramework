using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.PropertyAssigner;

public static class PropertyAssignerExtensions
{
    public static CodeStatement GetAssignStatementBySource(this IPropertyAssigner propertyAssigner, PropertyInfo property, CodeExpression sourceObjectRef, CodeExpression targetObjectRef)
    {
        if (propertyAssigner is null) throw new ArgumentNullException(nameof(propertyAssigner));
        if (property is null) throw new ArgumentNullException(nameof(property));
        if (sourceObjectRef is null) throw new ArgumentNullException(nameof(sourceObjectRef));
        if (targetObjectRef is null) throw new ArgumentNullException(nameof(targetObjectRef));


        return propertyAssigner.GetAssignStatement(property, sourceObjectRef.ToPropertyReference(property), targetObjectRef.ToPropertyReference(property));
    }

    public static IPropertyAssigner<TConfiguration> WithConfiguration<TConfiguration>(this IPropertyAssigner propertyAssigner, TConfiguration configuration)
            where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        if (propertyAssigner is null) throw new ArgumentNullException(nameof(propertyAssigner));
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));

        var genericAssigner = propertyAssigner as IPropertyAssigner<TConfiguration>;

        if (genericAssigner is not null && genericAssigner.Configuration == configuration)
        {
            return genericAssigner;
        }
        else
        {
            return new ImplPropertyAssigner<TConfiguration>(configuration, propertyAssigner);
        }
    }

    private class ImplPropertyAssigner<TConfiguration>(TConfiguration configuration, IPropertyAssigner innerAssigner)
        : PropertyAssigner<TConfiguration>(configuration, innerAssigner.DomainType!, innerAssigner.FileType)
        where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        public override CodeStatement GetAssignStatement(PropertyInfo property, CodeExpression sourcePropertyRef, CodeExpression targetPropertyRef)
        {
            if (property is null) throw new ArgumentNullException(nameof(property));
            if (sourcePropertyRef is null) throw new ArgumentNullException(nameof(sourcePropertyRef));
            if (targetPropertyRef is null) throw new ArgumentNullException(nameof(targetPropertyRef));

            return innerAssigner.GetAssignStatement(property, sourcePropertyRef, targetPropertyRef);
        }
    }
}

