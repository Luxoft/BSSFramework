namespace Framework.Configuration;

public class SystemConstant<T>
{
    public SystemConstant(string code, T defaultValue, string description)
    {
        this.Code = code ?? throw new ArgumentNullException(nameof(code));
        this.DefaultValue = defaultValue;
        this.Description = description;
    }


    public readonly string Code;

    public readonly T DefaultValue;

    public readonly string Description;
}
