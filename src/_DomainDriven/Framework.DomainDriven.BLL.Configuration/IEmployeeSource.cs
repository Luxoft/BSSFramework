using Framework.Configuration;

namespace Framework.DomainDriven.BLL.Configuration;

public interface IEmployeeSource
{
    IQueryable<IEmployee> GetQueryable();
}
