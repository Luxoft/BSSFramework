using System.CodeDom;

using Framework.Application.Domain;
using Framework.CodeDom;
using Framework.CodeGeneration.Configuration._Container;
using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class CodeDomExtensions
{
    public static CodeTypeReference GetIdentityObjectTypeRef<TConfiguration>(this IGeneratorConfigurationContainer<TConfiguration> fileFactory)
            where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return typeof(IIdentityObject<>).ToTypeReference(fileFactory.Configuration.Environment.GetIdentityType().ToTypeReference());
    }
}
