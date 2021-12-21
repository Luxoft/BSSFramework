namespace Framework.Persistent
{
    public interface ICodeObject<out TCode>
    {
        TCode Code { get; }
    }

    public interface ICodeObject : ICodeObject<string>
    {

    }
}