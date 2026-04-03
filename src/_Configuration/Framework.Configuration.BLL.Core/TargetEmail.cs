namespace Framework.Configuration.BLL;

public struct TargetEmail(string email, TargetEmailType targetEmailType)
{
    public string Email = email;
    public TargetEmailType TargetEmailType = targetEmailType;

    /// <summary>
    /// Конструктор c TargetEmailType = TargetEmailType.To
    /// </summary>
    /// <param name="email"></param>
    public TargetEmail(string email)
            : this(email, TargetEmailType.To)
    {
    }
}
