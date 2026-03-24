namespace Framework.Application.Session.DALObject;

public record DalObject<T>(T @Object, long ApplyIndex) : IdalObject
    where T : class
{
    Type IdalObject.Type { get; } = typeof(T);

    object IdalObject.Object => this.Object;
}

public record DalObject(object Object, Type Type, long ApplyIndex) : IdalObject;
