using Framework.Authorization.Domain;
using Framework.Validation;

namespace Framework.Authorization.BLL;

public partial class SecurityDomainBLLBase<TDomainObject>
{
    private void ExecuteBasePersist(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Recalculate(domainObject);
        this.Validate(domainObject, AuthorizationOperationContext.Save);
    }

    protected internal virtual void Recalculate(TDomainObject domainObject)
    {

    }

    protected internal virtual void Validate(TDomainObject domainObject, AuthorizationOperationContext operationContext)
    {
        this.Context.Validator.Validate(domainObject, (int)operationContext);
    }

    internal protected void Save(TDomainObject domainObject, bool validate)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (validate) { this.Save(domainObject); }
        else { base.Save(domainObject); }
    }

    public override void Insert(TDomainObject domainObject, Guid id)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.ExecuteBasePersist(domainObject);
        base.Insert(domainObject, id);
    }

    public override void Save(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.ExecuteBasePersist(domainObject);
        base.Save(domainObject);
    }
}
