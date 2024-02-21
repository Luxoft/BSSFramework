namespace Framework.DomainDriven.ImpersonateService;

public interface IImpersonateService
{
    Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func);
}
