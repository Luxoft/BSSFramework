namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public static class DomainSecurityServiceRootBuilderExtensions
{
    public static IDomainSecurityServiceRootBuilder AddDisabled<TDomainObject>(this IDomainSecurityServiceRootBuilder rootBuilder)
    {
        return rootBuilder.Add<TDomainObject>(b => b.SetView(SecurityOperation.Disabled).SetEdit(SecurityOperation.Disabled));
    }
}
