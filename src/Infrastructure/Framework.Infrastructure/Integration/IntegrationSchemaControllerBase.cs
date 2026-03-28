using System.Globalization;
using System.Net.Mime;

using Framework.Core;

using Microsoft.AspNetCore.Mvc;

using SecuritySystem;

namespace Framework.Infrastructure.Integration;

[Obsolete("Will be removed in v19")]
public abstract class IntegrationSchemaControllerBase(
    ISecuritySystem securitySystem,
    TimeProvider timeProvider,
    IEventXsdExporter2 eventXsdExporter)
    : ControllerBase
{
    private readonly TimeProvider timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    private const string AuthIntegrationNamespace = "http://authorization.luxoft.com/IntegrationEvent";

    [HttpGet]
    [Route("DownloadKnownTypesWsdl")]
    public Task<IActionResult> DownloadKnownTypesWsdl(CancellationToken cancellationToken) =>
            this.DownloadKnownTypesWsdl(this.IntegrationNamespace, this.GetEventDTOTypes(), cancellationToken);

    private async Task<IActionResult> DownloadKnownTypesWsdl(string xsdNamespace, IReadOnlyCollection<Type> eventTypes, CancellationToken cancellationToken)
    {
        await securitySystem.CheckAccessAsync(SecurityRole.SystemIntegration, cancellationToken);

        var content = eventXsdExporter.Export(xsdNamespace, "IntegrationEvent", eventTypes);

        var contentType = MediaTypeNames.Application.Octet;

        var fileName =
                $"KnowTypes {xsdNamespace} ({this.timeProvider.GetToday().ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}).zip";

        return this.File(content, contentType, fileName);
    }

    [HttpGet]
    [Route("DownloadAuthKnownTypesWsdl")]
    public Task<IActionResult> DownloadAuthKnownTypesWsdl(CancellationToken cancellationToken) =>
            this.DownloadKnownTypesWsdl(AuthIntegrationNamespace, this.GetAuthEventDTOTypes(), cancellationToken);

    protected abstract string IntegrationNamespace
    {
        get;
    }

    protected abstract IReadOnlyCollection<Type> GetEventDTOTypes();

    protected abstract IReadOnlyCollection<Type> GetAuthEventDTOTypes();
}
