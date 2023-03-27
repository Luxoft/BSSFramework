namespace Framework.Security;

[AttributeUsage(AttributeTargets.Field)]
public abstract class ApproveOperationAttribute : Attribute
{
    public readonly Enum Operation;


    protected ApproveOperationAttribute(Enum operation)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        this.Operation = operation;
    }
}
