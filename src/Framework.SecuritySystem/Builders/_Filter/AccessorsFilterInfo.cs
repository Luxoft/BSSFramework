namespace Framework.SecuritySystem.Builders._Factory;

public record AccessorsFilterInfo<TDomainObject>(Func<TDomainObject, IEnumerable<string>> GetAccessorsFunc);
