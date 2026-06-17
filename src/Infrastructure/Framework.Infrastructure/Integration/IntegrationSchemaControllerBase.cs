using System.Globalization;
using System.Net.Mime;

using Anch.SecuritySystem;

using Framework.Core;

using Microsoft.AspNetCore.Mvc;

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
    public Task<IActionResult> DownloadKnownTypesWsdl(CancellationToken ct) =>
            this.DownloadKnownTypesWsdl(this.IntegrationNamespace, this.GetEventDTOTypes(), ct);

    private async Task<IActionResult> DownloadKnownTypesWsdl(string xsdNamespace, IReadOnlyCollection<Type> eventTypes, CancellationToken ct)
    {
        await securitySystem.CheckAccessAsync(SecurityRole.SystemIntegration, ct);

        var content = eventXsdExporter.Export(xsdNamespace, "IntegrationEvent", eventTypes);

        var contentType = MediaTypeNames.Application.Octet;

        var fileName =
                $"KnownTypes {xsdNamespace} ({this.timeProvider.GetToday().ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}).zip";

        return this.File(content, contentType, fileName);
    }

    [HttpGet]
    [Route("DownloadAuthKnownTypesWsdl")]
    public Task<IActionResult> DownloadAuthKnownTypesWsdl(CancellationToken ct) =>
            this.DownloadKnownTypesWsdl(AuthIntegrationNamespace, this.GetAuthEventDTOTypes(), ct);

    protected abstract string IntegrationNamespace
    {
        get;
    }

    protected abstract IReadOnlyCollection<Type> GetEventDTOTypes();

    protected abstract IReadOnlyCollection<Type> GetAuthEventDTOTypes();
}

