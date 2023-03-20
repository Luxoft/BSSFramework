using System;
using System.CodeDom;
using Framework.CodeDom;

using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Server;

public static class ServerGeneratorConfigurationExtensions
{
    public static CodeMemberProperty GetVersionObjectPrivateImplementation(this IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return new CodeMemberProperty
               {
                       Name = "Version",
                       PrivateImplementationType = configuration.GetVesionObjectCodeTypeReference(),
                       GetStatements =
                       {
                               new CodeThisReferenceExpression().ToPropertyReference(configuration.VersionProperty).ToMethodReturnStatement()
                       },
                       Type = configuration.VersionType.ToTypeReference()
               };
    }



    public static CodeTypeReference GetVesionObjectCodeTypeReference(this IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return typeof(IVersionObject<>).MakeGenericType(configuration.VersionType).ToTypeReference();
    }
}
