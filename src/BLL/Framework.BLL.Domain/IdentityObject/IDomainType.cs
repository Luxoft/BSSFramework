using Framework.Application.Domain;

namespace Framework.BLL.Domain.IdentityObject;

public interface IDomainType : IVisualIdentityObject
{
    string NameSpace { get; }
}
