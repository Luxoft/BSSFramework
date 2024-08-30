namespace Framework.SecuritySystem.Builders._Factory;

public interface ISecurityFilterFactory<TDomainObject> : IFilterFactory<TDomainObject, SecurityFilterInfo<TDomainObject>>;
