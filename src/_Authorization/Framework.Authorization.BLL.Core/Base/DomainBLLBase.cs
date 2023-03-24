using System;

using Framework.Authorization.Domain;
using Framework.Validation;

namespace Framework.Authorization.BLL;

public partial class DomainBLLBase<TDomainObject, TOperation>
{
    private void ExecuteBasePersist(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Validate(domainObject, AuthorizationOperationContext.Save);
    }


    protected internal virtual void Validate(TDomainObject domainObject, AuthorizationOperationContext context)
    {
        this.Context.Validator.Validate(domainObject, (int)context);
    }

    internal void Save(TDomainObject domainObject, bool validate)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (validate) { this.Save(domainObject); }
        else          { base.Save(domainObject); }
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
