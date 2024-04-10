using System.Reflection;

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityRoleSource : ISecurityRoleSource
{
    private readonly Lazy<List<SecurityRole>> lazySecurityRoleTypeInfoList;

    public SecurityRoleSource(IEnumerable<SecurityRoleTypeInfo> securityRoleTypeInfoList)
    {
        this.lazySecurityRoleTypeInfoList = LazyHelper.Create(
            () =>
            {
                var result = GetSecurityRoles(securityRoleTypeInfoList)
                             .GetAllElements(sr => sr.Children)
                             .Distinct() // distinct by memory reference
                             .OrderBy(sr => sr.Name)
                             .ToList();

                ValidateDuplicates(result);

                return result;
            });
    }

    public IReadOnlyList<SecurityRole> SecurityRoles => this.lazySecurityRoleTypeInfoList.Value;

    private static IEnumerable<SecurityRole> GetSecurityRoles(IEnumerable<SecurityRoleTypeInfo> securityRoleTypeInfoList)
    {
        return from securityRoleTypeInfo in securityRoleTypeInfoList

               from securityRole in securityRoleTypeInfo.SecurityRoleType.GetStaticPropertyValueList<SecurityRole>()

               select securityRole;
    }

    private static void ValidateDuplicates(IReadOnlyList<SecurityRole> securityRoles)
    {
        var idDuplicates = securityRoles
                           .GetDuplicates(
                               new EqualityComparerImpl<SecurityRole>(
                                   (sr1, sr2) => sr1.Id == sr2.Id,
                                   sr => sr.Id.GetHashCode())).ToList();

        if (idDuplicates.Any())
        {
            throw new Exception($"SecurityRole 'Id' duplicates: {idDuplicates.Join(", ", sr => sr.Id)}");
        }

        var nameDuplicates = securityRoles
                             .GetDuplicates(
                                 new EqualityComparerImpl<SecurityRole>(
                                     (sr1, sr2) => sr1.Name == sr2.Name,
                                     sr => sr.Name.GetHashCode())).ToList();

        if (nameDuplicates.Any())
        {
            throw new Exception($"SecurityRole 'Name' duplicates: {nameDuplicates.Join(", ", sr => sr.Name)}");
        }
    }
}
