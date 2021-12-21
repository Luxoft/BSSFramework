using System;
using System.Collections.Generic;
using System.Linq;
using Framework.DomainDriven.UnitTest.Mock.StubProxy;

namespace Framework.Authorization.BLL.Tests.Unit.Support
{
    public class BLLFactoryContainerConfiguration
    {
        private readonly IList<Action<IOverrideMethodBuilder<IAuthorizationBLLFactoryContainer>>> _initializeActions = new List<Action<IOverrideMethodBuilder<IAuthorizationBLLFactoryContainer>>>();

        public BLLFactoryContainerConfiguration(params Action<IOverrideMethodBuilder<IAuthorizationBLLFactoryContainer>>[] initializeActions)
        {
            this._initializeActions = initializeActions.ToList();
        }

        public Action<IOverrideMethodBuilder<IAuthorizationBLLFactoryContainer>>[] InitializeActions
        {
            get { return this._initializeActions.ToArray(); }
        }

        public void Register(Action<IOverrideMethodBuilder<IAuthorizationBLLFactoryContainer>> initAction)
        {
            this._initializeActions.Add(initAction);
        }
    }
}