namespace Framework.DomainDriven.DBGenerator;

public class DbUserCredential
{
    private readonly string name;

    private readonly string password;

    internal DbUserCredential(string name, string password)
    {
        var parameters = new[] { name, password };
        if (parameters.Any(z => null == z) && parameters.Any(z => null != z))
        {
            throw new ArgumentException(
                                        $"Incorrect parameter values: userName:'{this.name}', password:'{password}'");
        }

        this.name = name;
        this.password = password;
    }

    public bool IsDefault => null == this.password && null == this.name;

    public virtual string UserName => this.name;

    public virtual string Password => this.password;

    public static DbUserCredential CreateDefault() => new DbUserCredential(null, null);

    public static DbUserCredential Create(string name, string password) => new DbUserCredential(name, password);
}
