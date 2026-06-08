namespace Framework.Application;

public interface IDalGenericInterceptor<in TDomainObject>
{
    Task SaveAsync(TDomainObject data, CancellationToken ct);

    Task RemoveAsync(TDomainObject data, CancellationToken ct);
}
