using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;

namespace Framework.BLL.Domain.Exceptions.BusinessLogic.NotFound;

public class ObjectByCodeNotFoundException<TCode> : BusinessLogicException
{
    public ObjectByCodeNotFoundException(Type type, TCode code)
            : base($"{type.Name} with code = \"{code}\" not found")
    {

    }
}
