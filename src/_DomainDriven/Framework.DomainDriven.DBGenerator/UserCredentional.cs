namespace Framework.DomainDriven.DBGenerator;

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

    private readonly string _name;
    private readonly string _password;

    internal UserCredentional(string name, string password)
    {
        var parameters = new[] { name, password };
        if (parameters.Any(z => null == z) && parameters.Any(z => null != z))
        {
            throw new ArgumentException(
                                        $"Incorrect parameter values: userName:'{this._name}', password:'{password}'");
        }

        this._name = name;
        this._password = password;
    }

    public bool IsDefault
    {
        get { return null == this._password && null == this._name; }
    }
    public virtual string UserName { get { return this._name; } }
    public virtual string Password{get { return this._password; }}

}
