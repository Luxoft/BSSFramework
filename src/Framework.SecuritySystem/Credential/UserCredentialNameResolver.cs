using Framework.Core;

namespace Framework.SecuritySystem.Credential;

public class RootUserCredentialNameResolver(IEnumerable<IUserCredentialNameByIdResolver> resolvers) : IUserCredentialNameResolver
{
    public virtual string GetUserName(UserCredential userCredential)
    {
        switch (userCredential)
        {
            case UserCredential.NamedUserCredential v:
                return v.Name;

            case UserCredential.IdentUserCredential v:
            {
                var request = from resolver in resolvers

                              let userName = resolver.TryGetUserName(v.Id)

                              where userName != null

                              select userName;

                return request.Distinct().Single(
                    () => new Exception($"{nameof(UserCredential)} with id {v.Id} not found"),
                    names => new Exception($"More one {nameof(UserCredential)} with id {v.Id}: {names.Join(", ", name => $"\"{name}\"")}"));
            }

            default: throw new ArgumentOutOfRangeException(nameof(userCredential));
        }
    }
}
