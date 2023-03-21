using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class AuditGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>, IAuditGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IAuditGenerationEnvironmentBase
{
    protected AuditGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
    }


    public override string ImplementClassName { get; } = "AuditFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Audit";


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        foreach (var dtoType in domainType.GetViewDTOTypes())
        {
            yield return new GetObjectWithRevisionMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
        }

        yield return new GetObjectRevisionsMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType);

        yield return new GetObjectPropertyRevisionsMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, this.Environment.AuditDTO);

        yield return new GetObjectPropertyRevisionsByDateRangeMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, this.Environment.AuditDTO);
    }
}
