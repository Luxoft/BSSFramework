namespace Framework.Exceptions;

public class ObjectByCodeNotFoundException<TCode> : BusinessLogicException
{
    public ObjectByCodeNotFoundException(Type type, TCode code)
            : base($"{type.Name} with code = \"{code}\" not found")
    {

    }
}
