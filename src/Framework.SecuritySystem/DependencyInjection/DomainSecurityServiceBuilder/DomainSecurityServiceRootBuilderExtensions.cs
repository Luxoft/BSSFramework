namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public static class DomainSecurityServiceRootBuilderExtensions
{
    public static IDomainSecurityServiceRootBuilder Add<TDomainObject>(
        this IDomainSecurityServiceRootBuilder rootBuilder,
        SecurityOperation viewOperation,
        SecurityPath<TDomainObject> securityPath)
    {
        return rootBuilder.Add(viewOperation, null, securityPath);
    }

    public static IDomainSecurityServiceRootBuilder Add<TDomainObject>(
        this IDomainSecurityServiceRootBuilder rootBuilder,
        SecurityOperation viewOperation,
        SecurityOperation editOperation = null,
        SecurityPath<TDomainObject> securityPath = null)
    {
        return rootBuilder.Add<TDomainObject>(
            b =>
            {
                b.SetView(viewOperation);

                if (editOperation != null)
                {
                    b.SetEdit(editOperation);
                }

                if (securityPath != null)
                {
                    b.SetPath(securityPath);
                }
            });
    }

    public static IDomainSecurityServiceRootBuilder AddViewDisabled<TDomainObject>(this IDomainSecurityServiceRootBuilder rootBuilder)
    {
        return rootBuilder.Add<TDomainObject>(SecurityOperation.Disabled);
    }

    public static IDomainSecurityServiceRootBuilder AddFullDisabled<TDomainObject>(this IDomainSecurityServiceRootBuilder rootBuilder)
    {
        return rootBuilder.Add<TDomainObject>(SecurityOperation.Disabled, SecurityOperation.Disabled);
    }
}
