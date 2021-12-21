using System;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Audit
{
    public interface IAuditDTOGeneratorConfigurationBase<out TEnvironment> : IAuditDTOGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment>
        where TEnvironment : IAuditDTOGenerationEnvironmentBase
    {
    }

    public interface IAuditDTOGeneratorConfigurationBase : IGeneratorConfiguration
    {
        string DomainObjectPropertiesRevisionDTOFullTypeName { get; }

        string DomainObjectPropertiesRevisionDTOTypeName { get; }

        string PropertyRevisionTypeName { get; }

        string PropertyRevisionFullTypeName { get; }
    }
}
