namespace Framework.SecuritySystem.Builders._Factory;

public record SecurityFilterInfo<TDomainObject>(
    Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc,
    Func<TDomainObject, bool> HasAccessFunc);
