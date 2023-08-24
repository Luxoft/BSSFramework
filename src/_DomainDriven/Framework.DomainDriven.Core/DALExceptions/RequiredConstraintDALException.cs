namespace Framework.DomainDriven.DALExceptions;

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
}
