#nullable enable
using System.Collections.Generic;
using System.Data;

using Framework.Cap.Abstractions;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

namespace SampleSystem.WebApiCore.Env.Database;

public class SampleSystemNHibSessionFactory : NHibSessionFactory
{
    private readonly ICapTransactionManager manager;

    public SampleSystemNHibSessionFactory(
            NHibConnectionSettings connectionSettings,
            IUserAuthenticationService userAuthenticationService,
            IEnumerable<IMappingSettings> mappingSettings,
            IDateTimeService dateTimeService,
            ICapTransactionManager manager)
            : base(connectionSettings, mappingSettings, userAuthenticationService, dateTimeService) =>
            this.manager = manager;

    public override void ProcessTransaction(IDbTransaction dbTransaction)
    {
        base.ProcessTransaction(dbTransaction);
        this.manager.Enlist(dbTransaction);
    }
}
