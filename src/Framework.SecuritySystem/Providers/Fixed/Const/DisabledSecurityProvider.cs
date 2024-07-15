namespace Framework.SecuritySystem;

public class DisabledSecurityProvider<TDomainObject>() : ConstSecurityProvider<TDomainObject>(true);
