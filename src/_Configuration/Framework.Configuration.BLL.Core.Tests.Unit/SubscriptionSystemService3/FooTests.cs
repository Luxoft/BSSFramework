using System;
using NUnit.Framework;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3
{
    [TestFixture]
    public class FooTests
    {
        [Test]
        public void FooLambdaTest()
        {
            ISubscriptionLambdaFunc<object, object> booleanLambda = new BooleanLambda();
            var func = booleanLambda.Lambda;
            var result = (bool) func("Hello");
            Console.WriteLine(result);
        }
    }

    public interface ISubscriptionLambdaFunc<in T, out TResult>
    {
        Func<T, TResult> Lambda { get; }
    }

    public abstract class SubscriptionLambdaFunc<T, TResult> : ISubscriptionLambdaFunc<object, object>
    {
        public abstract Func<T, TResult> Lambda { get; }

        Func<object, object> ISubscriptionLambdaFunc<object, object>.Lambda
        {
            get { return o => this.Lambda((T) o); }
        }
    }

    public class BooleanLambda : SubscriptionLambdaFunc<string, bool>
    {
        public override Func<string, bool> Lambda
        {
            get { return s => s.Length == 5; }
        }
    }
}