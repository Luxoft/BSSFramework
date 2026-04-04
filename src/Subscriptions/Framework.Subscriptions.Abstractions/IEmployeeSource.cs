using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions;

public interface IEmployeeSource
{
    IQueryable<IEmployee> GetQueryable();
}
