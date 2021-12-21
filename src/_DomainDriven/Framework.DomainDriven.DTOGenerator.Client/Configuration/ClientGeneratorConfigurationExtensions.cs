using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public static class ClientGeneratorConfigurationExtensions
    {
        public static CodeTypeReference GetBaseAbstractInterfaceReference(this IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration.GetCodeTypeReference(configuration.Environment.DomainObjectBaseType, ClientFileType.BaseAbstractInterfaceDTO);
        }

        public static CodeTypeReference GetBasePersistentInterfaceReference(this IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration.GetCodeTypeReference(configuration.Environment.PersistentDomainObjectBaseType, ClientFileType.BasePersistentInterfaceDTO);
        }

        public static CodeTypeReference GetBaseAuditPersistentInterfaceReference(this IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return configuration.GetCodeTypeReference(configuration.Environment.AuditPersistentDomainObjectBaseType, ClientFileType.BaseAuditPersistentInterfaceDTO);
        }

        public static bool IsReused(this IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> configuration, Type type)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (type == null) throw new ArgumentNullException(nameof(type));

            return configuration.ReuseTypesAssemblies.Union(DefaultReusedAssemblies).Contains(type.Assembly);
        }



        private static readonly ReadOnlyCollection<Assembly> DefaultReusedAssemblies =

            new[] { typeof(object).Assembly }.ToReadOnlyCollection();

    }
}