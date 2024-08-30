namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityFilterFactory<TDomainObject>(
    Framework.SecuritySystem.Builders.QueryBuilder.SecurityFilterBuilderFactory<TDomainObject> queryFactory,
    Framework.SecuritySystem.Builders.MaterializedBuilder.SecurityFilterBuilderFactory<TDomainObject> hasAccessFactory)
    : Framework.SecuritySystem.Builders.MixedBuilder.SecurityFilterFactory<TDomainObject>(queryFactory, hasAccessFactory);
