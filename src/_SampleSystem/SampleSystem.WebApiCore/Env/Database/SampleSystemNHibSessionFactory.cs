#nullable enable
using System.Collections.Generic;
using System.Data;

using Framework.Cap.Abstractions;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.NHibernate.Audit;

namespace SampleSystem.WebApiCore.Env.Database;

public class SampleSystemNHibSessionFactory : NHibSessionConfiguration
{
    private readonly ICapTransactionManager manager;

    public SampleSystemNHibSessionFactory(
            NHibConnectionSettings connectionSettings,
            IEnumerable<IMappingSettings> mappingSettings,
            IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService,
            ICapTransactionManager manager)
            : base(connectionSettings, mappingSettings, auditRevisionUserAuthenticationService) =>
            this.manager = manager;

    public override void ProcessTransaction(IDbTransaction dbTransaction)
    {
        base.ProcessTransaction(dbTransaction);
        this.manager.Enlist(dbTransaction);
    }
}
