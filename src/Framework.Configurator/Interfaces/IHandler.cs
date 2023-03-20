using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Interfaces;

public interface IHandler
{
    Task Execute(HttpContext context, CancellationToken cancellationToken);
}
