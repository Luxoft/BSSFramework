namespace Framework.Application.Domain;

public interface IVersionObject<out TVersion>
{
    TVersion Version { get; }
}
