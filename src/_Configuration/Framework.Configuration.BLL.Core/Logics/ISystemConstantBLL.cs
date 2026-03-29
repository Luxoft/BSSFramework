using Framework.Application.ApplicationVariable;

namespace Framework.Configuration.BLL;

public partial interface ISystemConstantBLL
{
    T GetValue<T>(ApplicationVariable<T> systemConstant);
}
