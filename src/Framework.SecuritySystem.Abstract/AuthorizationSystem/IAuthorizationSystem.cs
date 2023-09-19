namespace Framework.SecuritySystem;

public interface IAuthorizationSystem : IOperationAccessor
{
    public string CurrentPrincipalName { get; }
}
