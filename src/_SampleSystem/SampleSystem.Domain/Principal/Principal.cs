using Framework.Core;
using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain;

/// <summary>
/// #IADFRAME-1300
/// Если в основной системе есть объект Principal c полей ExternalId:string
/// То при генерации базы данных аудита получается PrincipalAudit.ExternalId : guid
/// Так как такая точно таблица и поле есть в Auth
/// </summary>
[BLLViewRole]
[BLLSaveRole]
public class Principal : AuditPersistentDomainObjectBase
{
    private string externalId;

    public virtual string ExternalId
    {
        get { return this.externalId; }
        set { this.externalId = value.TrimNull(); }
    }
}
