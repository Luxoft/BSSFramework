namespace Framework.Database.EntityFramework;

public interface IDbContextTypeSource
{
    public const string PrimaryKey = "PrimaryDbContextType";

    public const string SecondaryKey = "SecondaryDbContextType";

    Type GetDbContextType(Type domainObjectType);

    Type PrimaryDbContextType { get; }
}
