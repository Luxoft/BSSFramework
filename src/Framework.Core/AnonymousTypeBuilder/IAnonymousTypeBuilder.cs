namespace Framework.Core.AnonymousTypeBuilder;

public interface IAnonymousTypeBuilder<in TMap>
{
    Type GetAnonymousType(TMap sourceType);
}

public interface IIAnonymousTypeBuilderContainer<in TMap>
{
    IAnonymousTypeBuilder<TMap> AnonymousTypeBuilder { get; }
}
