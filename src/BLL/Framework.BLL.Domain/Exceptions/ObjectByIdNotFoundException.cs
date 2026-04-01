namespace Framework.BLL.Domain.Exceptions;

public class ObjectByIdNotFoundException<TIdent>(Type type, TIdent id) : BusinessLogicException($"{type.Name} with id = \"{id}\" not found");

public class ObjectByIdNotFoundException(Type type, Guid id) : ObjectByIdNotFoundException<Guid>(type, id);
