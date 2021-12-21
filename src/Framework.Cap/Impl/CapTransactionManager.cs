using System.Data;

using DotNetCore.CAP;

using Framework.Cap.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Cap.Impl;

public class CapTransactionManager : ICapTransactionManager
{
    private readonly ICapPublisher capPublisher;

    public CapTransactionManager(ICapPublisher capPublisher) => this.capPublisher = capPublisher;

    public void Enlist(IDbTransaction dbTransaction)
    {
        // Enlist current transaction to CAP
        var capTransaction = ActivatorUtilities.CreateInstance<SqlServerCapTransaction>(this.capPublisher.ServiceProvider);
        capTransaction.DbTransaction = dbTransaction;

        this.capPublisher.Transaction.Value = capTransaction;
    }
}
