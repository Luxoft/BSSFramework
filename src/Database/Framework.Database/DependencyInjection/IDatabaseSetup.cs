using System.Data;
using System.Linq.Expressions;

namespace Framework.Database.DependencyInjection;

public interface IDatabaseSetup
{
    bool AddDefaultListener { get; set; }

    IDatabaseSetup AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener;

    IDatabaseSetup AddVisitorContainer<TExpressionVisitorContainer>()
        where TExpressionVisitorContainer : class, IExpressionVisitorContainer;

    IDatabaseSetup AddVisitor(ExpressionVisitor expressionVisitor);

    IDatabaseSetup SetIsolationLevel(IsolationLevel isolationLevel);

    IDatabaseSetup SetBatchSize(int batchSize);

    IDatabaseSetup SetCommandTimeout(int timeout);

    IDatabaseSetup SetDefaultConnectionString(string connectionString);

    IDatabaseSetup SetDefaultConnectionStringName(string connectionStringName);
}
