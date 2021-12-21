using System.Collections.Generic;
using System.Reflection;

namespace Framework.Authorization.BLL
{
    public partial interface IEntityTypeBLL
    {
        void Register(IEnumerable<Assembly> assemblies);
    }
}