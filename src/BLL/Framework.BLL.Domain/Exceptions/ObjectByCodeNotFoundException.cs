namespace Framework.BLL.Domain.Exceptions;

public class ObjectByCodeNotFoundException<TCode>(Type type, TCode code) : BusinessLogicException($"{type.Name} with code = \"{code}\" not found");
