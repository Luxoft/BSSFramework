using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;

namespace Framework.BLL.Domain.Exceptions.BusinessLogic.NotFound;

public class ObjectByNameNotFoundException : BusinessLogicException
{
    public ObjectByNameNotFoundException(Type type, string name)
            : base($"{type.Name} with name = \"{name}\" not found")
    {

    }
}
