using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Transfering;
using System.Reflection;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.BLL;

namespace Framework.Workflow.BLL.Tests.Unit.BLLTests
{
    [TestClass]
    public class BaseBLLUnitTest : BaseUnitTest
    {
        //[TestMethod]
        //public void TestGetLoadParamsBLLBase()
        //{
        //    var context = GetContext();

        //    foreach (var dtoType in Enum.GetValues(typeof(MainDTOType)))
        //    {
        //        Assert.IsNotNull(context.Logics.StateBase.GetLoadParams((MainDTOType)dtoType));
        //    }

        //}

        //protected bool IsGenericInterfaceDerived(Type t)
        //{
        //    if (t.GetInterfaces().Any(x =>
        //                              x.IsGenericType &&
        //                              x.GetGenericTypeDefinition() == typeof (ILoadParamsFactory<>)))
        //    {
        //        return true;
        //    }

        //    return t.BaseType != null && IsGenericInterfaceDerived(t.BaseType);
        //}

        //[TestMethod]
        //public void GetLoadParamsGenericTest()
        //{
        //    var context = GetContext();
        //    var BLLArray = context.Logics.GetType().GetProperties().Where(p => IsGenericInterfaceDerived(p.PropertyType)).ToArray();

        //    foreach (var propertyInfo in BLLArray)
        //    {
        //        var propValue = context.Logics.GetType().GetProperty(propertyInfo.Name).GetValue(context.Logics, null);

        //        foreach (var enumVal in Enum.GetValues(typeof(MainDTOType)))
        //        {
        //            var result = propValue.GetType().GetMethod("GetLoadParams", new Type[1] { typeof(MainDTOType) }).Invoke(propValue, new object[] { enumVal });
        //            Assert.IsNotNull(result);
        //        }

        //    }
        //}
    }
}
