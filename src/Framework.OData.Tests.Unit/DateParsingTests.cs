using FluentAssertions;

using Framework.QueryLanguage;

using NUnit.Framework;

namespace Framework.OData.Tests.Unit;

[TestFixture]
public class DateParsingTests
{
    private IQueryable<TestClass> stream;

    [SetUp]
    public void Setup()
    {
        this.stream = new[]
                      {
                              new TestClass
                              {
                                      Id = 1,
                                      EndDateNull = new DateTime(2018, 8, 1),
                                      StartDateNotNull = new DateTime(2018, 8, 1),
                              },
                              new TestClass
                              {
                                      Id = 2,
                                      EndDateNull = new DateTime(2018, 8, 2),
                                      StartDateNotNull = new DateTime(2018, 8, 2)
                              },
                              new TestClass
                              {
                                      Id = 3,
                                      EndDateNull = null,
                                      StartDateNotNull = new DateTime(2018, 8, 3)
                              },
                              new TestClass
                              {
                                      Id = 4,
                                      EndDateNull = new DateTime(2018, 9, 10),
                                      StartDateNotNull = new DateTime(2018, 9, 10)
                              }
                      }.AsQueryable();
    }

    [Test]
    public void NotNullableDate__Month_Equal_Value_ElementFounded()
    {
        // Arrange
        var query = "$top=70&$filter=month(StartDateNotNull) eq 9";

        // Act
        var res = this.ParseAndProcess(query);

        // Arrange
        res.Id.Should().Be(4);
    }

    [Test]
    public void NotNullableDate__Day_Equal_Value_ElementFounded()
    {
        // Arrange
        var query = "$top=70&$filter=day(StartDateNotNull) eq 10";

        // Act
        var res = this.ParseAndProcess(query);

        // Arrange
        res.Id.Should().Be(4);
    }

    [Test]
    public void NotNullableDate_Equal_Value_ElementFounded()
    {
        // Arrange
        var query = "$top=70&$filter=StartDateNotNull eq datetime'2018-08-01'";

        // Act
        var res = this.ParseAndProcess(query);

        // Arrange
        res.Id.Should().Be(1);
    }

    [Test]
    public void NotNullableDate_Equal_Null_Exception()
    {
        // Arrange
        var query = "$top=70&$filter=StartDateNotNull eq null";

        // Act
        var res = Assert.Throws<InvalidOperationException>(() => this.ParseAndProcess(query));

        // Arrange
        res.Message.Should().Be("Sequence contains no elements");
    }

    [Test]
    public void NullableDate_Equal_Null_ElementFounded()
    {
        // Arrange
        var query = "$top=70&$filter=EndDateNull eq null";

        // Act
        var res = this.ParseAndProcess(query);

        // Arrange
        res.Id.Should().Be(3);
    }

    [Test]
    public void NullableDate_Equal_Value_Null_ElementFounded()
    {
        // Arrange
        var query = "$top=70&$filter=EndDateNull eq datetime'2018-08-01'";

        // Act
        var res = this.ParseAndProcess(query);

        // Arrange
        res.Id.Should().Be(1);
    }

    private TestClass ParseAndProcess(string query)
    {
        var selectOperation = SelectOperation.Parse(query);

        var selectOperationGeneric = new StandartExpressionBuilder().ToTyped<TestClass>(selectOperation);

        var res = selectOperationGeneric.Process(this.stream).Single();

        return res;
    }

    public class TestClass
    {
        public int Id { get; set; }

        public DateTime StartDateNotNull { get; set; }

        public DateTime? EndDateNull { get; set; }
    }
}
