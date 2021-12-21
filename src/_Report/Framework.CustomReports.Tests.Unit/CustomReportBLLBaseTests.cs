using System;

using FluentAssertions;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.CustomReports.BLL;
using Framework.CustomReports.Domain;

using NUnit.Framework;

namespace Framework.Configuration.Core.Tests.Unit
{
    [TestFixture]
    public sealed class CustomReportBLLBaseTests
    {
        [Test]
        public void ConvertToTypedValue_GuidString_TypedResult()
        {
            // Arrange
            const string GuidString = "e4c8f336-5b97-4de8-a5a6-1b25f2df1f9c";
            var bll = new TestingCustomReportBll(null);

            // Act
            var typedValue = bll.ConvertToTypedValue(GuidString, typeof(Guid));

            // Assert
            typedValue.Should().BeOfType<Guid>();
            typedValue.ToString().Should().Be(GuidString);
        }

        [Test]
        public void ConvertToTypedValue_EnumString_TypedResult()
        {
            // Arrange
            const string EnumString = "InternalTransaction";
            var bll = new TestingCustomReportBll(null);

            // Act
            var typedValue = bll.ConvertToTypedValue(EnumString, typeof(TransactionMessageMode));

            // Assert
            typedValue.Should().BeOfType<TransactionMessageMode>();
            typedValue.ToString().Should().Be(EnumString);
        }

        private class TestingCustomReportBll : CustomReportBLLBase<IConfigurationBLLContext, ConfigurationSecurityOperationCode, PersistentDomainObjectBase, object, object>
        {
            public TestingCustomReportBll(IConfigurationBLLContext context)
                : base(context)
            {
            }

            public new object ConvertToTypedValue(string value, Type targetType)
            {
                return base.ConvertToTypedValue(value, targetType);
            }

            protected override IReportStream GetStream(object report, object parameter)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
