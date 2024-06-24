using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial interface IEmployeeBLL
{
    Employee GetCurrent();
}
