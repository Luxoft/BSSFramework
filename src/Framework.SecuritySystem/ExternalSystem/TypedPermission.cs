namespace Framework.SecuritySystem.ExternalSystem;

public record TypedPermission(IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions, string PrincipalName) : IPermission
{
    public IEnumerable<Guid> GetRestrictions(Type securityContextType) => this.Restrictions.GetValueOrDefault(securityContextType, []);
}
