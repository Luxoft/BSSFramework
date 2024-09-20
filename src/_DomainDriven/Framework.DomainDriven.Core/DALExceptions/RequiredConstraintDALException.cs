namespace Framework.DomainDriven.DALExceptions;

public class RequiredConstraintDALException(DomainObjectInfo domainObjectInfo, string requiredPropertyName)
    : DALException<DomainObjectInfo>(domainObjectInfo, GetMessage(domainObjectInfo, requiredPropertyName))
{
    private static string GetMessage(DomainObjectInfo domainObjectInfo, string requiredPropertyName)
    {
        return $"The field '{requiredPropertyName}' of type {domainObjectInfo.Type.Name} must be initialized";
    }
}
