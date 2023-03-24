using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultBaseAuditPersistentDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultBaseAuditPersistentDTOFileFactory(TConfiguration configuration)
            : base(configuration, configuration.Environment.AuditPersistentDomainObjectBaseType)
    {
    }


    public override MainDTOFileType FileType { get; } = DTOGenerator.FileType.BaseAuditPersistentDTO;
}
