using AutoFixture.Idioms;

using FluentAssertions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.UnitTesting;
using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Lambdas
{
    [TestFixture]
    public sealed class LambdaProcessorFactoryTests : TestFixtureBase
    {
        private LambdaProcessorFactory<ITestBLLContext> factory;

        [SetUp]
        public void SetUp()
        {
            this.factory = new LambdaProcessorFactory<ITestBLLContext>(
                new TestBLLContext());
        }

        [Test]
        public void Create_ConditionLambdaProcessor_ProcessorCreated()
        {
            // Arrange

            // Act

            // Assert
            this.factory.Create<ConditionLambdaProcessor<ITestBLLContext>>().Should().NotBeNull();
        }
        

        [Test]
        public void Create_GenerationLambdaProcessorTo_ProcessorCreated()
        {
            // Arrange

            // Act

            // Assert
            this.factory.Create<GenerationLambdaProcessorTo<ITestBLLContext>>().Should().NotBeNull();
        }

        [Test]
        public void Create_GenerationLambdaProcessorCc_ProcessorCreated()
        {
            // Arrange

            // Act

            // Assert
            this.factory.Create<GenerationLambdaProcessorCc<ITestBLLContext>>().Should().NotBeNull();
        }

        [Test]
        public void Create_SecurityItemSourceLambdaProcessor_ProcessorCreated()
        {
            // Arrange

            // Act

            // Assert
            this.factory.Create<SecurityItemSourceLambdaProcessor<ITestBLLContext>>().Should().NotBeNull();
        }
    }
}
