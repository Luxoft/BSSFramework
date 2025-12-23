namespace Framework.Authorization.Environment.Validation;

[Flags]
public enum ValidatePermissionDelegateMode
{
    Role = 1,

    Period = 2,

    SecurityObjects = 4,

    All = Role + Period + SecurityObjects
}
