namespace Framework.DomainDriven.DALExceptions;

public class LinkedObjects
{
    private readonly Type source;
    private readonly Type target;
    private readonly string propertyName;

    public LinkedObjects(Type source, Type target, string propertyName)
    {
        this.source = source;
        this.propertyName = propertyName;
        this.target = target;
    }

    public Type Source
    {
        get { return this.source; }
    }

    public Type Target
    {
        get { return this.target; }
    }

    public string PropertyName
    {
        get { return this.propertyName; }
    }
}

public abstract class DALException<T> : DALException
{
    private readonly T args;

    protected DALException(T args, string message) : base(message)
    {
        this.args = args;
    }

    public T Args
    {
        get { return this.args; }
    }
}
