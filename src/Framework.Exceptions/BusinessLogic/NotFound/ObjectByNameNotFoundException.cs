namespace Framework.Exceptions;

public class ObjectByNameNotFoundException : BusinessLogicException
{
    public ObjectByNameNotFoundException(Type type, string name)
            : base($"{type.Name} with name = \"{name}\" not found")
    {

    }
}
