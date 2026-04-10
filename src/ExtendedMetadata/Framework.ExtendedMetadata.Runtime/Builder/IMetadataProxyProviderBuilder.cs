namespace Framework.ExtendedMetadata.Builder;

public interface IMetadataProxyProviderBuilder
{
    IMetadataProxyProviderBuilder Add<TDomainType>(Action<IDomainTypeMetadataBuilder<TDomainType>> setupAction);

    IMetadataProxyProvider Build();
}
