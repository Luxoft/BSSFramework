using System.CodeDom;

using Framework.Application.Domain;
using Framework.CodeDom.Extensions;

namespace Framework.CodeGeneration.DTOGenerator.Server.Configuration;

public static class ServerDTOGeneratorConfigurationExtensions
{
    public static CodeMemberProperty GetVersionObjectPrivateImplementation(this IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return new CodeMemberProperty
               {
                       Name = "Version",
                       PrivateImplementationType = configuration.GetVersionObjectCodeTypeReference(),
                       GetStatements =
                       {
                               new CodeThisReferenceExpression().ToPropertyReference(configuration.VersionProperty).ToMethodReturnStatement()
                       },
                       Type = configuration.VersionType.ToTypeReference()
               };
    }



    public static CodeTypeReference GetVersionObjectCodeTypeReference(this IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return typeof(IVersionObject<>).MakeGenericType(configuration.VersionType).ToTypeReference();
    }
}
