namespace Framework.Database.InlineAudit.DependencyInjection;

public record AuditValueResolverHeader<TProperty>(Type ResolverType)
{
    public static AuditValueResolverHeader<TProperty> Create<TResolver>()
        where TResolver : IAuditValueResolver<TProperty> => new(typeof(TResolver));
}

public static class AuditValueResolverHeader
{
    public static AuditValueResolverHeader<string> CurrentUser { get; } = AuditValueResolverHeader<string>.Create<CurrentUserAuditValueResolver>();

    public static AuditValueResolverHeader<DateTime> NowDateTime { get; } = AuditValueResolverHeader<DateTime>.Create<NowDateTimeAuditValueResolver>();
}
