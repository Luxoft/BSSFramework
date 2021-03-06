using System;

using Framework.Core;
using Framework.DomainDriven;
using Framework.Validation;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;

namespace SampleSystem.BLL.Test
{
    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public void TestFioValidation()
        {
            var map = new SampleSystemValidationMap(DynamicSource.Empty);

            var validator = new SampleSystemValidator(LazyInterfaceImplementHelper.CreateNotImplemented<ISampleSystemBLLContext>(), map.ToCompileCache());

            var fio = new Fio { FirstName = new string('A', 1000) };

            var obj = new TestObj ();

            var res = validator.GetValidationResult(obj, SampleSystemOperationContext.Request, null);

            return;
        }

        [TestMethod]
        public void TestValidationMap01()
        {
            var extendedData = Framework.DomainDriven.AvailableValues
                                                     .Infinity
                                                     .ToValidation()
                                                     .ToBLLContextValidationExtendedData<ISampleSystemBLLContext, Domain.PersistentDomainObjectBase, Guid>();

            var map1 = new SampleSystemValidationMap(extendedData);

            var map2 = map1.WithFixedTypes<Domain.DomainObjectBase>(typeof(Domain.DomainObjectBase).Assembly, typeof(Domain.DomainObjectFilterModel<>).Assembly);
        }

        [TestMethod]
        public void TestValidationMap02()
        {
            var extendedData = Framework.DomainDriven.AvailableValues
                                                     .Infinity
                                                     .ToValidation()
                                                     .ToBLLContextValidationExtendedData<ISampleSystemBLLContext, Domain.PersistentDomainObjectBase, Guid>();

            var map1 = new ValidationMap(extendedData);

            var map2 = map1.WithFixedTypes<Domain.DomainObjectBase>(typeof(Domain.DomainObjectBase).Assembly, typeof(Domain.DomainObjectFilterModel<>).Assembly);
        }
    }
}
