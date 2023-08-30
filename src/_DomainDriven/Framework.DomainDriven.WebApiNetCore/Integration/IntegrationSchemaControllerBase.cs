using System.Globalization;
using System.Net.Mime;

using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiNetCore.Integration;

[Obsolete("Will be removed in v19")]
public abstract class IntegrationSchemaControllerBase : ControllerBase
{
    private readonly IDateTimeService dateTimeService;

    private readonly IEventXsdExporter2 eventXsdExporter;

    private readonly IAuthorizationSystem authorizationSystem;

    private const string AuthIntegrationNamespace = "http://authorization.luxoft.com/IntegrationEvent";

    protected IntegrationSchemaControllerBase(
        IAuthorizationSystem authorizationSystem,
        IDateTimeService dateTimeService,
        IEventXsdExporter2 eventXsdExporter)
    {
        this.authorizationSystem = authorizationSystem;
        this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
        this.eventXsdExporter = eventXsdExporter;
    }

    [HttpGet]
    [Route("DownloadKnownTypesWsdl")]
    public IActionResult DownloadKnownTypesWsdl() =>
            this.DownloadKnownTypesWsdl(this.IntegrationNamespace, this.GetEventDTOTypes());

    private IActionResult DownloadKnownTypesWsdl(string xsdNamespace, IReadOnlyCollection<Type> eventTypes)
    {
        this.authorizationSystem.CheckAccess(new NonContextSecurityOperation<SecurityOperationCode>(SecurityOperationCode.SystemIntegration));

        var content = this.eventXsdExporter.Export(xsdNamespace, "IntegrationEvent", eventTypes);

        var contentType = MediaTypeNames.Application.Octet;

        var fileName =
                $"KnowTypes {xsdNamespace} ({this.dateTimeService.Today.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}).zip";

        return this.File(content, contentType, fileName);
    }

    [HttpGet]
    [Route("DownloadAuthKnownTypesWsdl")]
    public IActionResult DownloadAuthKnownTypesWsdl() =>
            this.DownloadKnownTypesWsdl(AuthIntegrationNamespace, this.GetAuthEventDTOTypes());

    protected abstract string IntegrationNamespace
    {
        get;
    }

    protected abstract IReadOnlyCollection<Type> GetEventDTOTypes();

    protected abstract IReadOnlyCollection<Type> GetAuthEventDTOTypes();
}
