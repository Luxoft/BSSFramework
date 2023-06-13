using ClosedXML.Excel;

using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record DownloadPermissionTemplateHandler
    (IRepositoryFactory<EntityType> RepositoryFactory) : IDownloadPermissionTemplateHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var contexts = await this.RepositoryFactory
                                 .Create()
                                 .GetQueryable()
                                 .Select(x => x.Name)
                                 .ToListAsync(cancellationToken);

        var assembly = this.GetType().Assembly;
        var resourceStream = assembly.GetManifestResourceStream("Framework.Configurator.Templates.Permissions.xlsx");
        using var workbook = new XLWorkbook(resourceStream);
        var worksheet = workbook.Worksheet(1);
        for (var i = 0; i < contexts.Count; i++)
        {
            var contextName = contexts[i];
            const int firstContentColumnIndex = 4;
            worksheet.Cell(1, firstContentColumnIndex + i).Value = contextName;
        }

        var ms = new MemoryStream();
        workbook.SaveAs(ms);
        ms.Position = 0;
        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        context.Response.Headers.ContentDisposition = "attachment; filename=\"permissions-template.xlsx\"";
        await ms.CopyToAsync(context.Response.Body, cancellationToken);
    }
}
