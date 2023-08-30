using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial interface ISystemConstantBLL
{
    T GetValue<T>(SystemConstant<T> systemConstant);

    IList<SystemConstant> Initialize(Type systemConstantContainerType);
}
