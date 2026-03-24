namespace Framework.Application.DALExceptions;

public class RequiredConstraintDalException(DomainObjectInfo domainObjectInfo, string requiredPropertyName)
    : DalException<DomainObjectInfo>(domainObjectInfo, GetMessage(domainObjectInfo, requiredPropertyName))
{
    private static string GetMessage(DomainObjectInfo domainObjectInfo, string requiredPropertyName)
    {
        return $"The field '{requiredPropertyName}' of type {domainObjectInfo.Type.Name} must be initialized";
    }
}
