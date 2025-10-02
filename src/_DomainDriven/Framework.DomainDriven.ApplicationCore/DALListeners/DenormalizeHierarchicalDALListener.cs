using System.Reflection;

using CommonFramework;
using CommonFramework.DictionaryCache;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class DenormalizeHierarchicalDALListener(IServiceProvider serviceProvider) : IBeforeTransactionCompletedDALListener
{
    public void Process(DALChangesEventArgs eventArgs)
    {
        throw new NotImplementedException("IvAt");
    }
}
