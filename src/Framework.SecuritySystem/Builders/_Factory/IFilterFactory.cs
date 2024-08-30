namespace Framework.SecuritySystem.Builders._Factory;

public interface IFilterFactory<TDomainObject, out TFilter>
{
    TFilter CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath);
}
