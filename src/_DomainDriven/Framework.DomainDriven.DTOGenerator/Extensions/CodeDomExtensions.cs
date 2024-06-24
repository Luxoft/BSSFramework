using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator;

public static class CodeDomExtensions
{
    public static CodeTypeReference GetIdentityObjectTypeRef<TConfiguration>(this IGeneratorConfigurationContainer<TConfiguration> fileFactory)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return typeof(IIdentityObject<>).ToTypeReference(fileFactory.Configuration.Environment.GetIdentityType().ToTypeReference());
    }
}
