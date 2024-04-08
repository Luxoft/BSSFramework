﻿using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Linq;
using Serilog;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationBusinessRoleInitializer : IAuthorizationBusinessRoleInitializer
{
    private readonly IRepository<BusinessRole> businessRoleRepository;

    private readonly ISecurityRoleSource securityRoleSource;

    private readonly ILogger logger;

    private readonly InitializeSettings settings;

    public AuthorizationBusinessRoleInitializer(
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<BusinessRole> businessRoleRepository,
        ISecurityRoleSource securityRoleSource,
        ILogger logger,
        InitializeSettings settings)
    {
        this.businessRoleRepository = businessRoleRepository;
        this.securityRoleSource = securityRoleSource;
        this.logger = logger.ForContext<AuthorizationBusinessRoleInitializer>();
        this.settings = settings;
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        await this.Init(this.securityRoleSource.SecurityRoles, cancellationToken);
    }

    public async Task Init(IEnumerable<SecurityRole> securityRoles, CancellationToken cancellationToken)
    {
        var dbRoles = await this.businessRoleRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeRoleResult = dbRoles.GetMergeResult(GetOrderedRoles(securityRoles), br => br.Id, sr => sr.Id);

        var mappingDict = new Dictionary<SecurityRole, BusinessRole>();

        if (mergeRoleResult.RemovingItems.Any())
        {
            switch (this.settings.UnexpectedAuthElementMode)
            {
                case UnexpectedAuthElementMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected roles in database: {mergeRoleResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthElementMode.Remove:
                {
                    foreach (var removingItem in mergeRoleResult.RemovingItems)
                    {
                        this.logger.Verbose("Remove Role: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

                        await this.businessRoleRepository.RemoveAsync(removingItem, cancellationToken);
                    }

                    break;
                }
            }
        }

        foreach (var securityRole in mergeRoleResult.AddingItems)
        {
            var businessRole = new BusinessRole
            {
                Name = securityRole.Name,
                Description = securityRole.Description
            };

            this.logger.Verbose("Create Role: {0} {1}", businessRole.Name, securityRole.Id);

            foreach (var subRole in securityRole.Children)
            {
                new SubBusinessRoleLink(businessRole) { SubBusinessRole = mappingDict[subRole] };
            }

            await this.businessRoleRepository.InsertAsync(businessRole, securityRole.Id, cancellationToken);

            mappingDict.Add(securityRole, businessRole);
        }

        foreach (var combinePair in mergeRoleResult.CombineItems)
        {
            var businessRole = combinePair.Item1;
            var securityRole = combinePair.Item2;

            businessRole.Description = securityRole.Description;

            var mergeSubRoleResult = businessRole.SubBusinessRoleLinks.GetMergeResult(
                securityRole.Children,
                link => link.SubBusinessRole.Id,
                child => child.Id);

            foreach (var child in mergeSubRoleResult.AddingItems)
            {
                new SubBusinessRoleLink(businessRole).SubBusinessRole = mappingDict[child];
            }

            foreach (var subBusinessRoleLink in mergeSubRoleResult.RemovingItems)
            {
                businessRole.RemoveDetail(subBusinessRoleLink);
            }

            businessRole.Description = securityRole.Description;

            await this.businessRoleRepository.SaveAsync(businessRole, cancellationToken);

            mappingDict.Add(securityRole, businessRole);
        }
    }

    private static IEnumerable<SecurityRole> GetOrderedRoles(IEnumerable<SecurityRole> securityRoles)
    {
        var startParts = securityRoles.Partial(sr => sr.Children.Any(), (rootRoles, leafRoles) => new { rootRoles, leafRoles });

        var orderedResult = startParts.leafRoles.ToList();

        var processed = startParts.leafRoles.ToHashSet();

        var unprocessed = new Queue<SecurityRole>(startParts.rootRoles);

        while (unprocessed.Any())
        {
            var currentRole = unprocessed.Dequeue();

            if (currentRole.Children.All(processed.Contains))
            {
                processed.Add(currentRole);
                orderedResult.Add(currentRole);
            }
            else
            {
                unprocessed.Enqueue(currentRole);
            }
        }

        return orderedResult;
    }
}
