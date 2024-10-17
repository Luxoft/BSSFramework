using Framework.Core;

namespace Framework.SecuritySystem.Credential;

public class RootUserCredentialNameResolver(IEnumerable<IUserCredentialNameByIdResolver> resolvers) : IUserCredentialNameResolver
{
    public virtual string GetUserName(UserCredential userCredential)
    {
        switch (userCredential)
        {
            case UserCredential.NamedUserCredential { Name: var name }:
                return name;

            case UserCredential.IdentUserCredential { Id: var id }:
            {
                var request = from resolver in resolvers

                              let userName = resolver.TryGetUserName(id)

                              where userName != null

                              select userName;

                return request.Distinct().Single(
                    () => new Exception($"{nameof(UserCredential)} with id {id} not found"),
                    names => new Exception($"More one {nameof(UserCredential)} with id {id}: {names.Join(", ", name => $"\"{name}\"")}"));
            }

            default: throw new ArgumentOutOfRangeException(nameof(userCredential));
        }
    }
}
