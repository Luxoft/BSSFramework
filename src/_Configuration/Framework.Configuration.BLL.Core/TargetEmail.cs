namespace Framework.Configuration.BLL;

public struct TargetEmail
{
    public string Email;
    public TargetEmailType TargetEmailType;

    /// <summary>
    /// Конструктор c TargetEmailType = TargetEmailType.To
    /// </summary>
    /// <param name="email"></param>
    public TargetEmail(string email)
            : this(email, TargetEmailType.To)
    {

    }

    public TargetEmail(string email, TargetEmailType targetEmailType)
            : this()
    {
        this.Email = email;
        this.TargetEmailType = targetEmailType;
    }
}
