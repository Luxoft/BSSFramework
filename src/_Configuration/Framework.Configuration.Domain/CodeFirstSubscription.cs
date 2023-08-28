using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Описатель подписки определяемой в коде.
/// Предназначен для управления активностью таких подписок во время исполнения.
/// </summary>
/// <seealso cref="DomainObjectBase" />
[BLLViewRole]
[BLLSaveRole(AllowCreate = false)]
[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.SubscriptionView)]
[ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.SubscriptionEdit)]
[UniqueGroup]
[NotAuditedClass]
public class CodeFirstSubscription : AuditPersistentDomainObjectBase, ICodeObject<string>
{
    private readonly string code;

    [NotPersistentField]
    private readonly DomainType domainType;

    protected CodeFirstSubscription()
    {
    }

    public CodeFirstSubscription(string code, DomainType domainType)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));
        }

        if (domainType == null)
        {
            throw new ArgumentNullException(nameof(domainType));
        }

        this.code = code;
        this.domainType = domainType;
    }

    /// <summary>
    /// Уникальное имя подписки
    /// </summary>
    [UniqueElement]
    [Required]
    [MaxLength(512)]
    public virtual string Code => this.code.TrimNull();

    /// <summary>
    /// Тип доменного объекта для которого должна срабатывать подписка.
    /// </summary>
    public virtual DomainType DomainType => this.domainType;

    [CustomSerialization(CustomSerializationMode.Normal)]
    public override bool Active
    {
        get { return base.Active; }
        set { base.Active = value; }
    }
}
