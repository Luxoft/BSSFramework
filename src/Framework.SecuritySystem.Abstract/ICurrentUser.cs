namespace Framework.SecuritySystem;

public interface ICurrentUser
{
    Guid Id { get; }

    string Name { get; }
}
