namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionRestriction
{
    Guid SecurityContextTypeId { get; }

    Guid SecurityContextId { get; }
}
