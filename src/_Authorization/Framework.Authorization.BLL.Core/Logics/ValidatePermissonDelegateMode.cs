namespace Framework.Authorization.BLL;

[Flags]
public enum ValidatePermissonDelegateMode
{
    Role = 1,

    Period = 2,

    SecurityObjects = 4,

    All = Role + Period + SecurityObjects
}
