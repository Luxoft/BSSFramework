using System;

namespace Framework.Exceptions;

public class ObjectByIdNotFoundException<TIdent> : BusinessLogicException
{
    public ObjectByIdNotFoundException(Type type, TIdent id)
            : base($"{type.Name} with id = \"{id}\" not found")
    {

    }
}

public class ObjectByIdNotFoundException : ObjectByIdNotFoundException<Guid>
{
    public ObjectByIdNotFoundException(Type type, Guid id)
            : base(type, id)
    {

    }
}
