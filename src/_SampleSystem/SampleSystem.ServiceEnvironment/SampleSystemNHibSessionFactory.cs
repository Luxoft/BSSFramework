#nullable enable

using System.Data;

using Framework.Cap.Abstractions;
using Framework.DomainDriven.DALExceptions;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.NHibernate.Audit;

namespace SampleSystem.ServiceEnvironment.Database;

public class SampleSystemNHibSessionEnvironment : NHibSessionEnvironment
{
    private readonly ICapTransactionManager manager;

    public SampleSystemNHibSessionEnvironment(
            NHibConnectionSettings connectionSettings,
            IEnumerable<IMappingSettings> mappingSettings,
            IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService,
            ICapTransactionManager manager,
            INHibSessionEnvironmentSettings settings,
            IDalValidationIdentitySource dalValidationIdentitySource)
                : base(connectionSettings, mappingSettings, auditRevisionUserAuthenticationService, settings, dalValidationIdentitySource) =>
            this.manager = manager;

    public override void ProcessTransaction(IDbTransaction dbTransaction)
    {
        base.ProcessTransaction(dbTransaction);
        this.manager.Enlist(dbTransaction);
    }
}
