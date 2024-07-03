namespace Framework.SecuritySystem;

public interface IOperationAccessor
{
    bool IsAdministrator() => this.HasAccess(SecurityRole.Administrator);

    bool HasAccess(SecurityRule.ExpandableSecurityRule securityRule);

    void CheckAccess(SecurityRule.ExpandableSecurityRule securityRule);
}
