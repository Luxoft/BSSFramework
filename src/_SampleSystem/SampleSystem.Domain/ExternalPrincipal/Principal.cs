using Framework.BLL.Domain.ServiceRole;

namespace SampleSystem.Domain.ExternalPrincipal;

/// <summary>
/// #IADFRAME-1300
/// If the main system has a Principal object with a field ExternalId:string
/// Then generating the audit database yields PrincipalAudit.ExternalId : guid
/// Because exactly such a table and field exist in Auth
/// </summary>
[BLLViewRole]
[BLLSaveRole]
public class Principal : AuditPersistentDomainObjectBase
{
    private string externalId = "";

    public virtual string ExternalId
    {
        get => this.externalId;
        set => this.externalId = value;
    }
}
