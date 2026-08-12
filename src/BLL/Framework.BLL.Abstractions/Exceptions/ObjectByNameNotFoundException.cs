using Framework.Application;

namespace Framework.BLL.Exceptions;

public class ObjectByNameNotFoundException(Type type, string name) : BusinessLogicException($"{type.Name} with name = \"{name}\" not found");
