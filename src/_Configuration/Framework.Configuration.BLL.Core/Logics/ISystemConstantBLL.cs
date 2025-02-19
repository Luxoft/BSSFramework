using Framework.ApplicationVariable;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface ISystemConstantBLL
{
    T GetValue<T>(ApplicationVariable<T> systemConstant);

    IList<SystemConstant> Initialize(Type systemConstantContainerType);
}
