namespace Framework.ExtendedMetadata.Builder;

public interface IExtendedAttributeSourceBuilder
{
    IExtendedAttributeSourceBuilder Add<TDomainType>(Action<IDomainTypeMetadataBuilder<TDomainType>> setupAction);

    ExtendedAttributeSource Build();
}
