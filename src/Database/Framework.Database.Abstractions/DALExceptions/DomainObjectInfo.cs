namespace Framework.Database.DALExceptions;

public readonly struct DomainObjectInfo(Type type, object objectIdent)
{
    public Type Type { get; } = type;

    public object ObjectIdent { get; } = objectIdent;
}
