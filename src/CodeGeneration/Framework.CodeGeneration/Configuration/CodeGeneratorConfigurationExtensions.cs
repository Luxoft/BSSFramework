using System.CodeDom;

using Framework.Application.Domain;
using Framework.CodeDom.Extensions;

namespace Framework.CodeGeneration.Configuration;

public static class CodeGeneratorConfigurationExtensions
{
    public static CodeTypeReference GetIdentityObjectCodeTypeReference(this ICodeGeneratorConfiguration<ICodeGenerationEnvironment> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return typeof(IIdentityObject<>).MakeGenericType(configuration.Environment.IdentityProperty.PropertyType).ToTypeReference();
    }
}
