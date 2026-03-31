using System.CodeDom;

using Framework.Application.Domain;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Extensions;

public static class CodeDomExtensions
{
    public static CodeTypeReference GetIdentityObjectTypeRef<TConfiguration>(this IFileGeneratorConfigurationContainer<TConfiguration> fileFactory)
            where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>
    {
        if (fileFactory == null) throw new ArgumentNullException(nameof(fileFactory));

        return typeof(IIdentityObject<>).ToTypeReference(fileFactory.Configuration.Environment.GetIdentityType().ToTypeReference());
    }
}
