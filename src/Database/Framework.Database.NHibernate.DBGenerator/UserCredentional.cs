namespace Framework.Database.NHibernate.DBGenerator;

[Obsolete("This type was renamed. Use UserCredential instead")]
public class UserCredentional
{
    public static UserCredentional CreateDefault()
    {
        return new UserCredentional(null, null);
    }
    public static UserCredentional Create(string name, string password)
    {
        return new UserCredentional(name, password);
    }

    private readonly string name;
    private readonly string password;

    internal UserCredentional(string name, string password)
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

    public bool IsDefault
    {
        get { return null == this.password && null == this.name; }
    }
    public virtual string UserName { get { return this.name; } }
    public virtual string Password{get { return this.password; }}

}
