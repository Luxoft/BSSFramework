namespace Framework.BLL.Domain.Persistent.IdentityObject;

public interface ICodeObject<out TCode>
{
    TCode Code { get; }
}

public interface ICodeObject : ICodeObject<string>
{

}
