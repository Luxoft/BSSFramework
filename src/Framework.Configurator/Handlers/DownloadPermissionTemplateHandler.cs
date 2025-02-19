using ClosedXML.Excel;

using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationSecurity;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class DownloadPermissionTemplateHandler(
    ISecurityContextInfoSource securityContextInfoSource,
    [CurrentUserWithoutRunAs]ISecuritySystem securitySystem)
    : IDownloadPermissionTemplateHandler
{
    private const int FirstContentColumnIndex = 4;

    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var assembly = this.GetType().Assembly;
        var resourceStream = assembly.GetManifestResourceStream("Framework.Configurator.Templates.Permissions.xlsx");
        using var workbook = new XLWorkbook(resourceStream);
        var worksheet = workbook.Worksheet(1);

        securityContextInfoSource.GetSecurityContextTypes().Foreach(
            (securityContextType, index) =>
                worksheet.Cell(1, FirstContentColumnIndex + index).Value = securityContextType.Name);

        await using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        ms.Position = 0;
        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        context.Response.Headers.ContentDisposition = "attachment; filename=\"permissions-template.xlsx\"";
        await ms.CopyToAsync(context.Response.Body, cancellationToken);
    }
}
