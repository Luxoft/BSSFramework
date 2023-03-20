using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

namespace Framework.Core.Tests.Unit;

public class SourceType11
{
    public SourceType2 InnerProp { get; set; }
}

public class SourceType12
{
    public Maybe<SourceType2> InnerProp { get; set; }
}

public class SourceType2
{
    public string Name { get; set; }

    public int Id { get; set; }
}

[TestFixture]
public class CustomTests
{
    [Test]
    public void TestLazyInterfaceImplement()
    {
        var res1 = LazyInterfaceImplementHelper<IEnumerable<string>>.CreateProxy(() => new[] { "a", "b", "c" }).ToArray();

        var res2 = LazyInterfaceImplementHelper<IEnumerable<int>>.CreateProxy(() => new[] { 1, 2, 3 }).ToArray();

        return;
    }

    [Test]
    public void TestMaybePlain()
    {
        //var plainExpander = new PlainTypeExpander("_", typeBuilder);

        //var rrr1 = this.TestMaybePlainR(new SourceType11 { InnerProp = null }, plainExpander);

        //var rrr111 = this.TestMaybePlainR(new SourceType11 { InnerProp = new SourceType2 { Name = null } }, plainExpander);

        var maybePlainTypeBuilder = new AnonymousTypeByPropertyBuilder<TypeMap, TypeMapMember>(new AnonymousTypeBuilderStorage("Test_MaybePlainTypeExpander")).WithCompressName().WithCache();

        var maybePlainExpander = new MaybePlainTypeExpander("_", maybePlainTypeBuilder);

        var rrr2 = this.TestMaybePlainR(new SourceType11 { InnerProp = null }, maybePlainExpander);

        var rrr3 = this.TestMaybePlainR(new SourceType12 { InnerProp = new Just<SourceType2>(null) }, maybePlainExpander);

        var rrr4 = this.TestMaybePlainR(new SourceType12 { InnerProp = new Just<SourceType2>(new SourceType2 { Name = "bla-bla", Id = 123 }) }, maybePlainExpander);

        Console.WriteLine(rrr2);
        Console.WriteLine(rrr3);
        Console.WriteLine(rrr4);

        return;
    }

    public object TestMaybePlainR<T>(T data, IPlainTypeExpander expander)
    {
        var convertExpr = expander.GetExpressionConverter(typeof(T)).GetConvertExpressionBase();

        var convertFunc = convertExpr.Compile();

        //-------------

        var res = convertFunc.DynamicInvoke(new object[] { data });

        return res;
    }
}
