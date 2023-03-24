// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITestBLLContext.cs" company="">
//
// </copyright>
// <summary>
//   Defines the ITestBLLContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Framework.DomainDriven.BLL.Tracking;

namespace Framework.DomainDriven.BLL.Security.Test.SecurityHierarchy.Domain;

using System;

public interface ITestBLLContext : IBLLBaseContextBase<PersistentDomainObjectBase, Guid>, ITrackingServiceContainer<PersistentDomainObjectBase>
{

}
