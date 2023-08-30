namespace Framework.DomainDriven.Lock;

[AttributeUsage(AttributeTargets.Field)]
public class GlobalLockAttribute : Attribute
{
    private readonly Type _domainType;

    public GlobalLockAttribute(Type domainType)
    {
        this._domainType = domainType;
    }

    public Type DomainType
    {
        get { return this._domainType; }
    }
}
