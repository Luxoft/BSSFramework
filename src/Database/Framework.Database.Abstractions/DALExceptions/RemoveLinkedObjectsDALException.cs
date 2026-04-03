namespace Framework.Database.DALExceptions;

public class RemoveLinkedObjectsDALException(LinkedObjects args, string message) : DALException<LinkedObjects>(args, message)
{
    public override string Message => $"{this.Args.Target.Name} cannot be removed because it is used in {this.Args.Source.Name}";
}
