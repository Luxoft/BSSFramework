namespace Framework.DomainDriven;

public interface IImpersonateService
{
    Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func);
}
