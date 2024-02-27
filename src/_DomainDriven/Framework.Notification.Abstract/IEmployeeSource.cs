namespace Framework.Notification;

public interface IEmployeeSource
{
    IQueryable<IEmployee> GetQueryable();
}
