using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.DomainServices;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using NHibernate.Linq;

using Serilog;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationOperationInitializer : IAuthorizationOperationInitializer
{
    private readonly IRepository<Operation> operationRepository;

    private readonly IRepository<BusinessRole> businessRoleRepository;

    private readonly IOperationDomainService operationDomainService;

    private readonly IBusinessRoleDomainService businessRoleDomainService;

    private readonly ISecurityOperationParser<Guid> securityOperationParser;

    private readonly ILogger logger;

    private readonly InitializeSettings settings;

    public AuthorizationOperationInitializer(
        IRepository<Operation> operationRepository,
        IOperationDomainService operationDomainService,
        ISecurityOperationParser<Guid> securityOperationParser,
        IRepository<BusinessRole> businessRoleRepository,
        IBusinessRoleDomainService businessRoleDomainService,
        ILogger logger,
        InitializeSettings settings)
    {
        this.operationRepository = operationRepository;
        this.operationDomainService = operationDomainService;
        this.securityOperationParser = securityOperationParser;
        this.businessRoleRepository = businessRoleRepository;
        this.businessRoleDomainService = businessRoleDomainService;
        this.logger = logger.ForContext<AuthorizationOperationInitializer>();
        this.settings = settings;
    }

    public async Task Init(CancellationToken cancellationToken)
    {
        var fullOperations = await this.InitMainSecurityOperations(cancellationToken);

        if (this.settings.InitDefaultAdminRole)
        {
            await this.InitAdminSecurityOperations(fullOperations, cancellationToken);
        }
    }

    private async Task<IReadOnlyDictionary<SecurityOperation<Guid>, Operation>> InitMainSecurityOperations(CancellationToken cancellationToken)
    {
        var dbOperations = await this.operationRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbOperations.GetMergeResult(
            this.securityOperationParser.Operations,
            operation => operation.Id,
            operation => operation.Id);

        if (mergeResult.RemovingItems.Any())
        {
            switch (this.settings.UnexpectedAuthElementMode)
            {
                case UnexpectedAuthElementMode.RaiseError:
                    throw new InvalidOperationException(
                        $"Unexpected security operations in database: {mergeResult.RemovingItems.Join(", ")}");

                case UnexpectedAuthElementMode.Remove:
                    {
                        foreach (var removingItem in mergeResult.RemovingItems)
                        {
                            this.logger.Verbose("Remove Operation: {RemovingItemName} {RemovingItemId}", removingItem.Name, removingItem.Id);

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

                this.logger.Verbose("Add Operation: {OperationName} {AttributeGuid}", newAuthOperation.Name, newAuthOperation.Id);
                await this.operationRepository.InsertAsync(newAuthOperation, securityOperation.Id, cancellationToken);

                return newAuthOperation;
            });

        foreach (var pair in mergeResult.CombineItems)
        {
            if (pair.Item1.Name != pair.Item2.Name || pair.Item1.Description != pair.Item2.Description)
            {
                pair.Item1.Name = pair.Item2.Name;
                pair.Item1.Description = pair.Item2.Description;

                await this.operationRepository.SaveAsync(pair.Item1, cancellationToken);
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
        var adminRole = await this.businessRoleDomainService.GetOrCreateEmptyAdminRole(cancellationToken);

        var mergeResult = adminRole.BusinessRoleOperationLinks.GetMergeResult(
            fullOperations.Where(pair => pair.Key.AdminHasAccess),
            operation => operation.Operation,
            pair => pair.Value);

        if (mergeResult.AddingItems.Any() || mergeResult.RemovingItems.Any())
        {
            mergeResult.AddingItems.Foreach(pair => new BusinessRoleOperationLink(adminRole) { Operation = pair.Value });

            mergeResult.RemovingItems.Foreach(link => adminRole.RemoveDetail(link));

            await this.businessRoleRepository.SaveAsync(adminRole, cancellationToken);
        }
    }
}
