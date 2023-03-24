using System;

using JetBrains.Annotations;

namespace Framework.DomainDriven;

public class DALObject<T> : IDALObject
        where T : class
{
    public DALObject(T @object, int applyIndex)
    {
        if (@object == null) throw new ArgumentNullException(nameof(@object));

        this.Object = @object;

        this.ApplyIndex = applyIndex;
    }


    public T Object { get; private set; }

    Type IDALObject.Type
    {
        get { return typeof(T); }
    }

    public long ApplyIndex { get; private set; }

    object IDALObject.Object
    {
        get { return this.Object; }
    }
}

public class DALObject : IDALObject
{
    public DALObject([NotNull] object @object, [NotNull] Type type, long applyIndex)
    {
        if (@object == null) throw new ArgumentNullException(nameof(@object));
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Object = @object;
        this.Type = type;
        this.ApplyIndex = applyIndex;
    }

    public object Object { get; private set; }

    public Type Type { get; private set; }

    public long ApplyIndex { get; private set; }
}
