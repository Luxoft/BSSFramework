namespace Framework.SecuritySystem;

public class AccessDeniedSecurityProvider<TDomainObject>() : ConstSecurityProvider<TDomainObject>(false);
