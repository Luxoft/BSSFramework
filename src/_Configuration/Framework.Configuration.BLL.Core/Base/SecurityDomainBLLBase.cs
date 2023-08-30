using Framework.Configuration.Domain;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public partial class SecurityDomainBLLBase<TDomainObject, TOperation>
{
    private void ExecuteBasePersist(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Validate(domainObject, ConfigurationOperationContext.Save);
    }


    private void Validate(TDomainObject domainObject, ConfigurationOperationContext context)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.GetValidationResult(domainObject, context).TryThrow();
    }

    protected virtual ValidationResult GetValidationResult(TDomainObject domainObject, ConfigurationOperationContext context)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return this.Context.Validator.GetValidationResult(domainObject, (int)context);
    }

    internal protected void Save(TDomainObject value, bool validate)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        if (validate) { this.Save(value); }
        else { base.Save(value); }
    }

    public override void Insert(TDomainObject value, Guid id)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        this.ExecuteBasePersist(value);
        base.Insert(value, id);
    }

    public override void Save(TDomainObject value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        this.ExecuteBasePersist(value);
        base.Save(value);
    }
}
