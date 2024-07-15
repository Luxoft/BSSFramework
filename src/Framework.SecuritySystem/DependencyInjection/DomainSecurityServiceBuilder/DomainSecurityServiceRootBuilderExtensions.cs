namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public static class DomainSecurityServiceRootBuilderExtensions
{
    public static IDomainSecurityServiceRootBuilder Add<TDomainObject>(
        this IDomainSecurityServiceRootBuilder rootBuilder,
        SecurityRule.DomainSecurityRule viewSecurityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        return rootBuilder.Add(viewSecurityRule, null, securityPath);
    }

    public static IDomainSecurityServiceRootBuilder Add<TDomainObject>(
        this IDomainSecurityServiceRootBuilder rootBuilder,
        SecurityRule.DomainSecurityRule viewSecurityRule,
        SecurityRule.DomainSecurityRule editSecurityRule = null,
        SecurityPath<TDomainObject> securityPath = null)
    {
        return rootBuilder.Add<TDomainObject>(
            b =>
            {
                b.SetView(viewSecurityRule);

                if (editSecurityRule != null)
                {
                    b.SetEdit(editSecurityRule);
                }

                if (securityPath != null)
                {
                    b.SetPath(securityPath);
                }
            });
    }

    public static IDomainSecurityServiceRootBuilder AddViewDisabled<TDomainObject>(this IDomainSecurityServiceRootBuilder rootBuilder)
    {
        return rootBuilder.Add<TDomainObject>(SecurityRule.Disabled);
    }

    public static IDomainSecurityServiceRootBuilder AddFullDisabled<TDomainObject>(this IDomainSecurityServiceRootBuilder rootBuilder)
    {
        return rootBuilder.Add<TDomainObject>(SecurityRule.Disabled, SecurityRule.Disabled);
    }
}
