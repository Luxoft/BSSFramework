using Framework.Authorization.Domain;

using SecuritySystem.Services;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationUserCredentialNameByIdResolver(IQueryableSource queryableSource)
    : IUserCredentialNameByIdResolver
{
    public string? TryGetUserName(Guid id)
    {
        return this.GetQueryable(id).Select(principal => (string?)principal.Name).SingleOrDefault();
    }

    private IQueryable<Principal> GetQueryable(Guid id) =>
        queryableSource.GetQueryable<Principal>().Where(principal => principal.Id == id);
}
