namespace Framework.Core.AnonymousTypeBuilder;

public interface ISwitchNameObject<out TOutputObject>
{
    TOutputObject SwitchName(string newName);
}
