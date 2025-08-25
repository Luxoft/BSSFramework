namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityFilterFactory<TDomainObject>(
    SecuritySystem.Builders.QueryBuilder.SecurityFilterBuilderFactory<TDomainObject> queryFactory,
    SecuritySystem.Builders.MaterializedBuilder.SecurityFilterBuilderFactory<TDomainObject> hasAccessFactory)
    : SecuritySystem.Builders.MixedBuilder.SecurityFilterFactory<TDomainObject>(queryFactory, hasAccessFactory);
