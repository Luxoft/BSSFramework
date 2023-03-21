using Framework.Core;
using Framework.Configuration.Domain;

namespace Framework.Configuration.Generated.DTO;

public partial class ConfigurationServerPrimitiveDTOMappingService
{
    public override void MapExceptionMessage(ExceptionMessageStrictDTO mappingObject, ExceptionMessage domainObject)
    {
        base.MapExceptionMessage(mappingObject, domainObject);

        domainObject.InnerException.Maybe(innerException =>
                                          {
                                              innerException.IsClient = true;
                                              innerException.IsRoot = false;
                                          });
    }
}
