using Framework.Validation;

namespace Framework.DomainDriven;

public class RequiredConstraintDALException : DALException<DomainObjectInfo>
{
    private readonly string _requiredPropertyName;

    public RequiredConstraintDALException(DomainObjectInfo domainObjectInfo, string requiredPropertyName)
            : base(domainObjectInfo, GetMessage(domainObjectInfo, requiredPropertyName))
    {
        this._requiredPropertyName = requiredPropertyName;
    }

    private static string GetMessage(DomainObjectInfo domainObjectInfo, string requiredPropertyName)
    {
        return $"The field '{requiredPropertyName}' of type {domainObjectInfo.Type.Name} must be initialized";
    }

    public override ValidationException Convert()
    {
        return new ValidationException(GetMessage(this.Args, this._requiredPropertyName));
    }
}
