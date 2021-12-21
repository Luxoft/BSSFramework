using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.OData;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.DomainDriven.SerializeMetadata.Tests.Unit
{
    [TestClass]
    public class MainUnitTests
    {
        [TestMethod]
        public void DuplicateDomainTypeSubset_NotThrowException()
        {
            // arrange
            var systemMetadataTypeBuilder = new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.Client, typeof(PersistentDomainObjectBase).Assembly);

            var selectOperation = SelectOperation.Parse($"$select={nameof(TestDomainObj.Prop1)},{nameof(TestDomainObj.Prop2)}");

            var domainTypeSubset = systemMetadataTypeBuilder.SystemMetadata.GetDomainTypeMetadataFullSubset(new TypeHeader(typeof(TestDomainObj)), selectOperation);

            // action

            Action action = () =>

            {
                var anonType1 = systemMetadataTypeBuilder.AnonymousTypeBuilder.GetAnonymousType(domainTypeSubset.OverrideHeader(th => new MyCustomTypeHeader(th.Name, "AAA")));

                var anonType2 = systemMetadataTypeBuilder.AnonymousTypeBuilder.GetAnonymousType(domainTypeSubset.OverrideHeader(th => new MyCustomTypeHeader(th.Name, "BBB")));

                return;
            };

            // assert

            action.Should().NotThrow();
        }

        [TestMethod]
        public void NotFoundPropertyDomainTypeSubset_NotThrowException()
        {
            // arrange
            var systemMetadataTypeBuilder = new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.Client, typeof(PersistentDomainObjectBase).Assembly);

            // action

            var values = new[]
                             {
                                 //"$select=Property1/Property/Name,Property2/Property3/Property4/Name",
                                 //"$select=Property1/Property/Name,Property2/Property3/Property4/Age"
                                 new { report = "report_1", Select = "$select=Property2/Name" },
                                 new { report = "report_2", Select = "$select=Property2/Age" },

                             }
                .Select(z => new { source = z, selectOperation = SelectOperation.Parse(z.Select) })
                .Select(z => new { source = z.source, selectOperation = z.selectOperation, domainTypeSubset = systemMetadataTypeBuilder.SystemMetadata.GetDomainTypeMetadataFullSubset(new TypeHeader(typeof(TestDomainObj)), z.selectOperation) })
                .Select(z => new { source = z.source, selectOperation = z.selectOperation, domainTypeSubset = z.domainTypeSubset, anonType = systemMetadataTypeBuilder.AnonymousTypeBuilder.GetAnonymousType(z.domainTypeSubset.OverrideHeader(th => new TestReportTypeHeader(th.Name, z.source.report))) })
                .ToList();


            foreach (var value in values)
            {
                var propertiesSet = value.source.Select.Substring("$select=".Length).Split(',');

                Action action = () =>
                                {
                                    foreach (var s in propertiesSet)
                                    {
                                        var lastPropertyType = s.Split('/').Aggregate(
                                            value.anonType,
                                            (type, propertyName) => type.GetProperty(propertyName, true).PropertyType);
                                    }
                                };

                action.Should().NotThrow();
            }

            return;
        }
    }
}
