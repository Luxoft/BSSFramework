namespace Framework.DomainDriven;

public record DALObject<T>(T @Object, long ApplyIndex) : IDALObject
    where T : class
{
    Type IDALObject.Type { get; } = typeof(T);

    object IDALObject.Object => this.Object;
}

public record DALObject(object Object, Type Type, long ApplyIndex) : IDALObject;
