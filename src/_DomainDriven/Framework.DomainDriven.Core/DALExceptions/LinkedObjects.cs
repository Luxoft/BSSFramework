namespace Framework.DomainDriven.DALExceptions;

public class LinkedObjects
{
    private readonly Type _source;
    private readonly Type _target;
    private readonly string _propertyName;

    public LinkedObjects(Type source, Type target, string propertyName)
    {
        this._source = source;
        this._propertyName = propertyName;
        this._target = target;
    }

    public Type Source
    {
        get { return this._source; }
    }

    public Type Target
    {
        get { return this._target; }
    }

    public string PropertyName
    {
        get { return this._propertyName; }
    }
}

public abstract class DALException<T> : DALException
{
    private readonly T _args;

    protected DALException(T args, string message) : base(message)
    {
        this._args = args;
    }

    public T Args
    {
        get { return this._args; }
    }
}
