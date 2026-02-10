namespace Framework.DomainDriven;

public interface IDalGenericInterceptor<in TDomainObject>
{
    Task SaveAsync(TDomainObject data, CancellationToken cancellationToken);

    Task RemoveAsync(TDomainObject data, CancellationToken cancellationToken);
}
