using Framework.Application.Domain;

namespace Framework.BLL.Domain.IdentityObject;

public interface ILambdaObject : IVisualIdentityObject
{
    string Value { get; }
}
