using Framework.Persistent;
using Framework.Configuration.Domain;

using GenericQueryable;
using GenericQueryable.Fetching;

namespace Framework.Configuration.BLL;

public partial class ControlSettingsBLL
{
    public ControlSettings? GetRootControlSettingsForCurrentPrincipal(string name)
    {
        var currentPrincipalName = this.Context.Authorization.CurrentUser.Name;

        return this.GetRootControlSettings(name, currentPrincipalName);
    }

    public ControlSettings? GetRootControlSettings(string name, string accountName)
    {
        var results = this.GetListBy(z => z.Name == name && z.AccountName == accountName && z.Parent == null, FullPropertyFetchRule);

        if (results.Count > 1)
        {
            //throw new BusinessLogicException("UtilitiesService has more than one setting:{0} for user:{1}", name, accountName);
            return results.OrderByDescending(z => z.ModifyDate).First();
        }

        return results.FirstOrDefault();
    }

    private static readonly FetchRule<ControlSettings> FullPropertyFetchRule =
        FetchRule<ControlSettings>
            .Create(z => z.Parent)
            .Fetch(z => z.ControlSettingsParams).ThenFetch(z => z.ControlSettingsParamValues);

    public void AddChild(ControlSettings parent, ControlSettings child)
    {
        child.Parent = parent;
        parent.AddDetail(child);
    }
}
