using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class ControlSettingsBLL : DomainBLLBase<ControlSettings>
    {
        public ControlSettingsBLL(IConfigurationBLLContext context)
            : base(context)
        {

        }

        public ControlSettings GetRootControlSettingsForCurrentPrincipal(string name)
        {
            var currentPrincipalName = this.Context.Authorization.RunAsManager.PrincipalName;

            return this.GetRootControlSettings(name, currentPrincipalName);
        }

        public ControlSettings GetRootControlSettings(string name, string accountName)
        {
            var results = this.GetListBy(z => z.Name == name && z.AccountName == accountName && z.Parent == null, this.GetFullPropertyLoadParamActions().ToArray());

            if (results.Count > 1)
            {
                //throw new BusinessLogicException("UtilitiesService has more than one setting:{0} for user:{1}", name, accountName);
                return results.OrderByDescending(z => z.ModifyDate).First();
            }

            return results.FirstOrDefault();
        }

        private IEnumerable<Expression<Action<IPropertyPathNode<ControlSettings>>>> GetFullPropertyLoadParamActions()
        {
            yield return q => q.Select(z => z.Parent);
            yield return q => q.SelectMany(z => z.ControlSettingsParams).SelectMany(z => z.ControlSettingsParamValues);
        }

        public void AddChild(ControlSettings parent, ControlSettings child)
        {
            child.Parent = parent;
            parent.AddDetail(child);
        }
    }
}
