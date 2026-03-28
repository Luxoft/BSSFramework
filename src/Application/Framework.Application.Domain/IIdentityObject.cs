namespace Framework.Application.Domain;

public interface IIdentityObject<out TIdent>
{
    TIdent Id { get; }
}
