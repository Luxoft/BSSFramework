using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;
using Framework.Security;

namespace Framework.DomainDriven.DTOGenerator;

public static class CodeDomExtensions
{
    public static CodeAttributeDeclaration ToCodeAttributeDeclaration(this DomainObjectAccessAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        return attribute.GetType().ToTypeReference().ToAttributeDeclaration(attribute.GetCodeAttributeArguments().ToArray());
    }

    private static IEnumerable<CodeAttributeArgument> GetCodeAttributeArguments(this DomainObjectAccessAttribute attribute)
    {
        if (attribute == null) throw new ArgumentNullException(nameof(attribute));

        var code = attribute.SecurityOperationCode;

        if (code != null)
        {
            yield return code.ToPrimitiveExpression().ToAttributeArgument();
        }
    }



    public static CodeTypeReference GetIdentityObjectTypeRef<TConfiguration>(this IGeneratorConfigurationContainer<TConfiguration> fileFactory)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return typeof(IIdentityObject<>).ToTypeReference(fileFactory.Configuration.Environment.GetIdentityType().ToTypeReference());
    }
}
