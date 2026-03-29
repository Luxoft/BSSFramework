namespace Framework.Core.TypeResolving;

[Flags]
public enum TypeSearchMode
{
    Name =  1,

    FullName = 2,

    Both = Name + FullName
}
