namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityFilterFactory<TDomainObject>(
    Anch.SecuritySystem.Builders.QueryBuilder.SecurityFilterBuilderFactory<TDomainObject> queryFactory,
    Anch.SecuritySystem.Builders.MaterializedBuilder.SecurityFilterBuilderFactory<TDomainObject> hasAccessFactory)
    : Anch.SecuritySystem.Builders.MixedBuilder.SecurityFilterFactory<TDomainObject>(queryFactory, hasAccessFactory);
