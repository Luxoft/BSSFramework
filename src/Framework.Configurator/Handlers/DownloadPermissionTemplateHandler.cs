using ClosedXML.Excel;
using ClosedXML.Report;

using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationCore.ExternalSource;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record DownloadPermissionTemplateHandler(
    ISecurityContextSource SecurityContextSource,
    [DisabledSecurity] IRepository<Permission> PermissionRepository,
    ISecurityEntitySource SecurityEntitySource,
    ISecuritySystem SecuritySystem) : IDownloadPermissionTemplateHandler
{
    private const int FirstContentColumnIndex = 6;
    private const string PermissionTableVariable = "PermissionsData";

    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var contextsDict = this.SecurityContextSource.SecurityContextTypes
            .Select(x => this.SecurityContextSource.GetSecurityContextInfo(x))
            .ToDictionary(x => x.Id, x => x.Name);


        var assembly = this.GetType().Assembly;
        var resourceStream = assembly.GetManifestResourceStream("Framework.Configurator.Templates.Permissions.xlsx");

        using var template = new XLTemplate(resourceStream);
        var worksheet = template.Workbook.Worksheet(1);

        // NOTE: one column is already in template
        worksheet.Column(FirstContentColumnIndex).InsertColumnsAfter(contextsDict.Count - 1);
        var i = 0;
        foreach (var contextName in contextsDict.Values)
        {
            var column = worksheet.Column(FirstContentColumnIndex + i);
            column.Cell(1).Value = contextName;
            column.Cell(2).Value = $"{{{{item.{nameof(PermissionDto.ContextItems)}[{i}]}}}}";
            i++;
        }

        var permissionDtos = await this.GetPermissionsAsync(contextsDict.Keys, cancellationToken);
        template.AddVariable(PermissionTableVariable, permissionDtos);
        template.Generate();

        context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        context.Response.Headers.ContentDisposition = "attachment; filename=\"permissions-template.xlsx\"";

        await using var ms = new MemoryStream();
        template.SaveAs(ms, true, true);
        ms.Position = 0;
        await ms.CopyToAsync(context.Response.Body, cancellationToken);
    }

    private async Task<IEnumerable<PermissionDto>> GetPermissionsAsync(IEnumerable<Guid> orderedContexts, CancellationToken cancellationToken)
    {
        var allPermissions = await this.PermissionRepository.GetQueryable()
            .Select(x => new { x.Id, Login = x.Principal.Name, Role = x.Role.Name, x.Period, x.Comment })
            .ToListAsync(cancellationToken);

        var allContexts = await this.PermissionRepository.GetQueryable()
            .SelectMany(x => x.Restrictions.Select(y => new PermissionContextInfo(y.SecurityContextId, y.SecurityContextType.Id, x.Id)))
            .ToListAsync(cancellationToken);

        var permissionsWithContextDict = GetPermissionDictWithContextEntityNames(allContexts.GroupBy(x => x.PermissionId));

        return allPermissions.Select(
            x =>
                new PermissionDto(
                    x.Id,
                    x.Login,
                    x.Role,
                    x.Period.StartDate,
                    x.Period.EndDate,
                    x.Comment,
                    OrderedContextsWithEntities(permissionsWithContextDict.GetValueOrDefault(x.Id)).ToArray()));

        IEnumerable<string> OrderedContextsWithEntities(Dictionary<Guid, string>? contextEntities) =>
            contextEntities != null
                ? orderedContexts.Select(ctxId => contextEntities.TryGetValue(ctxId, out var ctxName) ? ctxName : string.Empty)
                : orderedContexts.Select(_ => string.Empty);
    }

    private Dictionary<Guid, Dictionary<Guid, string>> GetPermissionDictWithContextEntityNames(IEnumerable<IGrouping<Guid, PermissionContextInfo>> groupedPermissions)
    {
        return groupedPermissions.ToDictionary(
            x => x.Key,
            x => GetContextsWithEntityNames(x.GroupBy(c => c.ContextId)).ToDictionary(c => c.ContextId, c => c.EntityNames));

        IEnumerable<(Guid ContextId, string EntityNames)> GetContextsWithEntityNames(IEnumerable<IGrouping<Guid, PermissionContextInfo>> groupedContextWithEntities) =>
            groupedContextWithEntities.Select(
                x => (x.Key, this.SecurityEntitySource.GetTyped(x.Key).GetSecurityEntitiesByIdents(x.Select(y => y.EntityId)).Select(e => e.Name).Join(", ")));
    }


    private sealed record PermissionContextInfo(Guid EntityId, Guid ContextId, Guid PermissionId);

    private sealed record PermissionDto(Guid Id, string Login, string Role, DateTime StartDate, DateTime? EndDate, string? Comment, string[] ContextItems);
}
