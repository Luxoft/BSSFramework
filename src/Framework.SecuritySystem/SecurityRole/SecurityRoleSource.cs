using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityRoleSource : ISecurityRoleSource
{
    private readonly IReadOnlyDictionary<Guid, FullSecurityRole> securityRoleByIdDict;

    private readonly IReadOnlyDictionary<string, FullSecurityRole> securityRoleByNameDict;


    private readonly IReadOnlyDictionary<string, FullSecurityRole> securityRoleByCustomNameDict;

    public SecurityRoleSource(IEnumerable<FullSecurityRole> securityRoles)
    {
        this.SecurityRoles = securityRoles.ToList();

        this.Validate();

        this.securityRoleByIdDict = this.SecurityRoles.Where(sr => !sr.IsVirtual).ToDictionary(v => v.Id);

        this.securityRoleByNameDict = this.SecurityRoles.ToDictionary(v => v.Name);

        this.securityRoleByCustomNameDict =
            this.SecurityRoles.Where(v => v.Information.CustomName != null).ToDictionary(v => v.Information.CustomName);
    }

    public IReadOnlyList<FullSecurityRole> SecurityRoles { get; }

    public FullSecurityRole GetFullRole(SecurityRole securityRole) => this.SecurityRoles.Single(sr => sr == securityRole);

    public FullSecurityRole GetSecurityRole(string name)
    {
        return this.securityRoleByNameDict.GetValueOrDefault(name)
               ?? this.securityRoleByCustomNameDict.GetValueOrDefault(name)
               ?? throw new Exception($"SecurityRole with name '{name}' not found");
    }

    public FullSecurityRole GetSecurityRole(Guid id)
    {
        return this.securityRoleByIdDict.GetValueOrDefault(id) ?? throw new Exception($"SecurityRole with id '{id}' not found");
    }

    private void Validate()
    {
        var idDuplicates = this.SecurityRoles
                           .GetDuplicates(
                               new EqualityComparerImpl<FullSecurityRole>(
                                   (sr1, sr2) => sr1.Id == sr2.Id,
                                   sr => sr.Id.GetHashCode())).ToList();

        if (idDuplicates.Any())
        {
            throw new Exception($"SecurityRole 'Id' duplicates: {idDuplicates.Join(", ", sr => sr.Id)}");
        }

        var nameDuplicates = this.SecurityRoles
                             .GetDuplicates(
                                 new EqualityComparerImpl<FullSecurityRole>(
                                     (sr1, sr2) => sr1.Name == sr2.Name,
                                     sr => sr.Name.GetHashCode())).ToList();

        if (nameDuplicates.Any())
        {
            throw new Exception($"SecurityRole 'Name' duplicates: {nameDuplicates.Join(", ", sr => sr.Name)}");
        }
    }
}
