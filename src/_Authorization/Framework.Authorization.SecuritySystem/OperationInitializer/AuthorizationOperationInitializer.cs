using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.DomainServices;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using NHibernate.Linq;

using Serilog;

namespace Framework.Authorization.SecuritySystem.OperationInitializer;

public class AuthorizationOperationInitializer : IAuthorizationOperationInitializer
{
    private readonly IRepository<Operation> operationRepository;

    private readonly IRepository<BusinessRole> businessRoleRepository;

    private readonly IOperationDomainService operationDomainService;

    private readonly IBusinessRoleDomainService businessRoleDomainService;

    private readonly ISecurityOperationParser securityOperationParser;

    public AuthorizationOperationInitializer(
        IRepository<Operation> operationRepository,
        IOperationDomainService operationDomainService,
        ISecurityOperationParser securityOperationParser,
        IRepository<BusinessRole> businessRoleRepository,
        IBusinessRoleDomainService businessRoleDomainService)
    {
        this.operationRepository = operationRepository;
        this.operationDomainService = operationDomainService;
        this.securityOperationParser = securityOperationParser;
        this.businessRoleRepository = businessRoleRepository;
        this.businessRoleDomainService = businessRoleDomainService;
    }

    public async Task InitSecurityOperations(UnexpectedAuthOperationMode mode, CancellationToken cancellationToken)
    {
        var fullOperations = await this.InitMainSecurityOperations(mode, cancellationToken);

        await this.InitAdminSecurityOperations(fullOperations, cancellationToken);
    }

    private async Task<IReadOnlyDictionary<SecurityOperation<Guid>, Operation>> InitMainSecurityOperations(UnexpectedAuthOperationMode mode, CancellationToken cancellationToken)
    {
        var operationRepository = this.operationRepository;

        var dbOperations = await operationRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbOperations.GetMergeResult(
            this.securityOperationParser
                .Operations
                .Cast<SecurityOperation<Guid>>(),
            operation => operation.Id,
            operation => operation.Id);

        if (mergeResult.RemovingItems.Any())
        {
            switch (mode)
            {
                case UnexpectedAuthOperationMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected security operations in database: {mergeResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthOperationMode.Remove:
                    {
                        foreach (var removingItem in mergeResult.RemovingItems)
                        {
                            Log.Verbose("Remove Operation: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

                            await this.operationDomainService.RemoveAsync(removingItem, cancellationToken);
                        }

                        break;
                    }
            }
        }

        var addingOperations = await mergeResult.AddingItems.ToDictionaryAsync(
            v => v,
            async securityOperation =>
            {
                var newAuthOperation = new Operation
                {
                    Name = securityOperation.Name,
                    Description = securityOperation.Description
                };

                Log.Verbose("Add Operation: {OperationName} {AttributeGuid}", newAuthOperation.Name, newAuthOperation.Id);
                await operationRepository.InsertAsync(newAuthOperation, securityOperation.Id, cancellationToken);

                return newAuthOperation;
            });

        foreach (var pair in mergeResult.CombineItems)
        {
            if (pair.Item1.Name != pair.Item2.Name || pair.Item1.Description != pair.Item2.Description)
            {
                pair.Item1.Name = pair.Item2.Name;
                pair.Item1.Description = pair.Item2.Description;

                await operationRepository.SaveAsync(pair.Item1, cancellationToken);
            }
        }

        var result = mergeResult.CombineItems.ToDictionary(pair => pair.Item2, pair => pair.Item1).Concat(addingOperations).ToDictionary();

        foreach (var pair in result)
        {
            var approveSecurityOperation = pair.Key.ApproveOperation as SecurityOperation<Guid>;

            if (pair.Value.ApproveOperation?.Id != approveSecurityOperation?.Id)
            {
                pair.Value.ApproveOperation = approveSecurityOperation.Maybe(v => result[v]);
            }
        }

        return result;
    }

    private async Task InitAdminSecurityOperations(IReadOnlyDictionary<SecurityOperation<Guid>, Operation> fullOperations, CancellationToken cancellationToken)
    {
        var businessRoleRepository = this.businessRoleRepository;

        var adminRole = await this.businessRoleDomainService.GetOrCreateEmptyAdminRole(cancellationToken);

        var mergeResult = adminRole.BusinessRoleOperationLinks.GetMergeResult(
            fullOperations.Where(pair => pair.Key.AdminHasAccess),
            operation => operation.Operation,
            pair => pair.Value);

        if (mergeResult.AddingItems.Any() || mergeResult.RemovingItems.Any())
        {
            mergeResult.AddingItems.Foreach(pair => new BusinessRoleOperationLink(adminRole) { Operation = pair.Value });

            mergeResult.RemovingItems.Foreach(link => adminRole.RemoveDetail(link));

            await businessRoleRepository.SaveAsync(adminRole, cancellationToken);
        }
    }
}
