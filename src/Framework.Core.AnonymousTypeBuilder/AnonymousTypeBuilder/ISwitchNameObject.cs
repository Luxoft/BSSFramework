namespace Framework.Core
{
    public interface ISwitchNameObject<out TOutputObject>
    {
        TOutputObject SwitchName(string newName);
    }
}