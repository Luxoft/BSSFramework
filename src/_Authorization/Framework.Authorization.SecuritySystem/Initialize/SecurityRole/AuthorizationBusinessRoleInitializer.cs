using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using NHibernate.Linq;

using Serilog;

namespace Framework.Authorization.SecuritySystem.Initialize;

public class AuthorizationBusinessRoleInitializer : IAuthorizationBusinessRoleInitializer
{
    private readonly IRepository<BusinessRole> businessRoleRepository;

    private readonly IRepository<Operation> operationRepository;

    private readonly ISecurityRoleSource securityRoleSource;

    private readonly ILogger logger;

    private readonly InitializeSettings settings;

    public AuthorizationBusinessRoleInitializer(
        IRepository<BusinessRole> businessRoleRepository,
        IRepository<Operation> operationRepository,
        ISecurityRoleSource securityRoleSource,
        ILogger logger,
        InitializeSettings settings)
    {
        this.businessRoleRepository = businessRoleRepository;
        this.operationRepository = operationRepository;
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

        var dbOperations = await this.operationRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeRoleResult = dbRoles.GetMergeResult(securityRoles, br => br.Id, sr => sr.Id);

        var dbOperationDict = dbOperations.ToDictionary(op => op.Id);

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

            foreach (var securityOperation in securityRole.Operations.Cast<SecurityOperation<Guid>>())
            {
                var operation = dbOperationDict[securityOperation.Id];

                new BusinessRoleOperationLink(businessRole).Operation = operation;

                this.logger.Verbose("Add operation {0} to Role: {1}", operation.Name, businessRole.Name);
            }

            await this.businessRoleRepository.InsertAsync(businessRole, securityRole.Id, cancellationToken);
        }

        foreach (var combinePair in mergeRoleResult.CombineItems)
        {
            var businessRole = combinePair.Item1;
            var securityRole = combinePair.Item2;

            businessRole.Description = securityRole.Description;

            var mergeOperationResult = businessRole.BusinessRoleOperationLinks.GetMergeResult(
                securityRole.Operations.Cast<SecurityOperation<Guid>>(),
                link => link.Operation.Id,
                operation => operation.Id);

            foreach (var securityOperation in mergeOperationResult.AddingItems)
            {
                var operation = dbOperationDict[securityOperation.Id];

                new BusinessRoleOperationLink(businessRole).Operation = operation;

                this.logger.Verbose("Add operation {0} to Role: {1}", operation.Name, businessRole.Name);
            }

            foreach (var dbOperationLink in mergeOperationResult.RemovingItems)
            {
                businessRole.RemoveDetail(dbOperationLink);

                this.logger.Verbose("Remove operation {0} from Role: {1}", dbOperationLink.Operation.Name, businessRole.Name);
            }

            businessRole.Description = securityRole.Description;

            await this.businessRoleRepository.SaveAsync(businessRole, cancellationToken);
        }
    }
}
